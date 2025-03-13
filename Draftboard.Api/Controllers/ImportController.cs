using Draftboard.Api.Csv.Records;
using Draftboard.Api.Data;
using Draftboard.Api.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Draftboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly DraftboardDbContext _context;

        public ImportController(DraftboardDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpPost]
        [Route("positions")]
        public async Task<IActionResult> ImportAsync([FromBody] IEnumerable<string> positions)
        {
            await _context.Positions.AddRangeAsync(positions.Select(x => new Position
            {
                Id = x
            }));
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("keepers")]
        public async Task<IActionResult> ImportKeepersAsync([FromBody] List<KeeperRecord> records)
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM FantasyTeamPlayers");
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM FantasyTeams");

            await _context.AddRangeAsync(records.Select(x => x.FantasyTeam).Distinct().Select(x =>
                new FantasyTeam {
                    Id = x
                }
            ));

            await _context.SaveChangesAsync();

            var fantasyTeamsById = _context.FantasyTeams.ToDictionary(x => x.Id);
            var playersByFantraxId = _context.Players.ToDictionary(x => x.FantraxId);
            await _context.FantasyTeamPlayers.AddRangeAsync(records.Select(x => new FantasyTeamPlayer
            {
                Contract = x.Contract,
                Salary = x.Salary,
                FantasyTeam = fantasyTeamsById[x.FantasyTeam],
                Player = playersByFantraxId[x.ID]
            }));

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("seasons/{season}/hittingstats")]
        public async Task<IActionResult> ImportAsync(int season, [FromBody] List<FantasyHittingStatsRecord> records)
        {
            var allPlayersByFantraxId = await _context.Players.ToDictionaryAsync(x => x.FantraxId);
            var allSeasonStatsByFantraxId = await _context.PlayerStats.Where(x => x.Season == season).ToDictionaryAsync(x => x.Player.FantraxId);

            foreach(var statRecord in records)
            {
                if(allSeasonStatsByFantraxId.TryGetValue(statRecord.ID, out var existingStats))
                {
                    existingStats.AtBats = statRecord.AB;
                    existingStats.Homeruns = statRecord.HR;
                    existingStats.OnBasePct = statRecord.OBP;
                    existingStats.NetStolenBases2 = statRecord.SBN2;
                    existingStats.RunsProduced = statRecord.RP;
                    existingStats.TotalBases = statRecord.TB;
                }
                else if(allPlayersByFantraxId.TryGetValue(statRecord.ID, out var player))
                {
                    existingStats = new PlayerStats
                    {
                        Season = season,
                        Player = player,
                        AtBats = statRecord.AB,
                        Homeruns = statRecord.HR,
                        NetStolenBases2 = statRecord.SBN2,
                        OnBasePct = statRecord.OBP,
                        RunsProduced = statRecord.RP,
                        TotalBases = statRecord.TB
                    };

                    await _context.AddAsync(existingStats);
                }
                else
                {
                    throw new Exception("Couldn't find existing stat or player. Make sure all players are imported.");
                }
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("seasons/{season}/pitchingstats")]
        public async Task<IActionResult> ImportAsync(int season, [FromBody] List<FantasyPitchingStatsRecord> records)
        {
            var allPlayersByFantraxId = await _context.Players.ToDictionaryAsync(x => x.FantraxId);
            var allSeasonStatsByFantraxId = await _context.PlayerStats.Where(x => x.Season == season).ToDictionaryAsync(x => x.Player.FantraxId);

            foreach (var statRecord in records)
            {
                if (allSeasonStatsByFantraxId.TryGetValue(statRecord.ID, out var existingStats))
                {
                    existingStats.InningsPitched = statRecord.IP;
                    existingStats.ERA = statRecord.ERA;
                    existingStats.WHIP = statRecord.WHIP;
                    existingStats.Strikeouts = statRecord.K;
                    existingStats.RPC = statRecord.RPC3;
                    existingStats.SPC = statRecord.SPC;
                }
                else if (allPlayersByFantraxId.TryGetValue(statRecord.ID, out var player))
                {
                    existingStats = new PlayerStats
                    {
                        Season = season,
                        Player = player,
                        InningsPitched = statRecord.IP,
                        ERA = statRecord.ERA,
                        WHIP = statRecord.WHIP,
                        Strikeouts = statRecord.K,
                        RPC = statRecord.RPC3,
                        SPC = statRecord.SPC
                    };

                    await _context.AddAsync(existingStats);
                }
                else
                {
                    throw new Exception("Couldn't find existing stat or player. Make sure all players are imported.");
                }
            }

            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPut]
        [Route("players")]
        public async Task<IActionResult> ImportAsync([FromBody] List<PlayerRecord> players, [FromQuery]bool freshImport = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE PlayerPositions");
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM FantasyTeamPlayers");

            var keptPlayers = players.Where(x => x.Status != "FA" && x.Status != "W" && !x.Status.StartsWith("W ("));
            if (freshImport)
            {
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM FantasyTeams");

                // Add teams
                await _context.AddRangeAsync(keptPlayers.Select(x => x.Status).Distinct().Select(x =>
                    new FantasyTeam
                    {
                        Id = x
                    }
                ));
            }

            await _context.SaveChangesAsync();

            // Add all players
            var positionsById = _context.Positions.ToDictionary(x => x.Id);
            var playersByFantraxId = _context.Players.ToDictionary(x => x.FantraxId);

            for(var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                if (playersByFantraxId.TryGetValue(player.Id, out var existingPlayer))
                {
                    // Update player
                    existingPlayer.Age = player.Age;
                    existingPlayer.MLBTeam = player.Team;
                    existingPlayer.Rank = i;
                    existingPlayer.Name = player.Player;
                }
                else
                {
                    // Create player
                    existingPlayer = new Player
                    {
                        FantraxId = player.Id,
                        Name = player.Player,
                        Rank = i,
                        MLBTeam = player.Team,
                        Age = player.Age
                    };
                    existingPlayer = (await _context.Players.AddAsync(existingPlayer)).Entity;
                }

                var playerPositions = player.Position.Split(',').ToHashSet();
                // If the player is eligible at any non-SP or RP position, add UT
                if (playerPositions.Any(x => x != "SP" && x != "RP"))
                {
                    playerPositions.Add("UT");
                }

                // If the player is eligible at either SP or RP position, add P
                if (playerPositions.Any(x => x == "SP" || x == "RP"))
                {
                    playerPositions.Add("P");
                }

                foreach (var playerPosition in playerPositions)
                {
                    await _context.PlayerPositions.AddAsync(new PlayerPosition
                    {
                        Player = existingPlayer,
                        Position = positionsById[playerPosition]
                    });
                }
            }

            await _context.SaveChangesAsync();

            // Add keepers
            var fantasyTeamsById = _context.FantasyTeams.ToDictionary(x => x.Id);
            playersByFantraxId = _context.Players.ToDictionary(x => x.FantraxId);
            await _context.FantasyTeamPlayers.AddRangeAsync(keptPlayers.Select(x => new FantasyTeamPlayer
            {
                Contract = x.Contract,
                Salary = x.Salary,
                FantasyTeam = fantasyTeamsById[x.Status],
                Player = playersByFantraxId[x.Id]
            }));

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
