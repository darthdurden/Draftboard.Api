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
    public class PlayersController : ControllerBase
    {
        private readonly DraftboardDbContext _context;

        public PlayersController(DraftboardDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayers([FromQuery]IEnumerable<string> positions, bool includeTaken = false)
        {
            IQueryable<Player> filteredPlayers = _context.Players;
            if (positions.Any())
            {
                filteredPlayers = filteredPlayers.Where(x => x.PlayerPositions.Any(x => positions.Contains(x.Position.Id)));
            }

            if(!includeTaken)
            {
                filteredPlayers = filteredPlayers.Where(x => x.FantasyTeamPlayer == null);
            }

            var players = await filteredPlayers.Select(x =>
            
                new PlayerDto
                {
                    Id = x.Id,
                    Age = x.Age,
                    MLBTeam = x.MLBTeam,
                    Name = x.Name,
                    Rank = x.Rank,
                    Positions = GetPositions(x.PlayerPositions.Select(y => y.Position.Id)),
                    HittingStats = x.PlayerStats.Where(x => x.Homeruns.HasValue).Select(y => new HittingStatsDto
                    {
                        Season = y.Season,
                        AtBats = y.AtBats ?? 0,
                        Homeruns = y.Homeruns ?? 0,
                        NetStolenBases2 = y.NetStolenBases2 ?? 0,
                        OnBasePct = y.OnBasePct ?? 0,
                        RunsProduced = y.RunsProduced ?? 0,
                        TotalBases = y.TotalBases ?? 0
                    }),
                    PitchingStats = x.PlayerStats.Where(x => x.ERA.HasValue).Select(y => new PitchingStatsDto
                    {
                        Season = y.Season,
                        InningsPitched = y.InningsPitched ?? 0,
                        ERA = y.ERA ?? 0,
                        ReliefPitcherContribution = y.RPC ?? 0,
                        StartingPitcherContribution = y.SPC ?? 0,
                        Strikeouts = y.Strikeouts ?? 0,
                        WHIP = y.WHIP ?? 0
                    }),
                    FantasyTeam = x.FantasyTeamPlayer != null ? x.FantasyTeamPlayer.FantasyTeam.Id : null,
                    Headshot = $"https://fantraximg.com/si/headshots/MLB/hs{x.FantraxId.Trim('*')}_400_6.png",
                    FirstName = x.Name.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[0],
                    LastName = string.Join(' ', x.Name.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Skip(1)),
                }).ToListAsync();

            return Ok(players);
        }

        public static IEnumerable<string> GetPositions(IEnumerable<string> positions)
        {
            var offensivePositions = new string[] { "C", "1B", "2B", "SS", "3B", "OF" };
            var pitchingPositions = new string[] { "SP", "RP" };
            var positionsHash = new HashSet<string>(positions);
            if (positionsHash.Any(x => offensivePositions.Contains(x)))
            {
                positionsHash.Add("UT");
            }

            if(positionsHash.Any(x => pitchingPositions.Contains(x)))
            {
                positionsHash.Add("P");
            }

            return positionsHash;
        }
    }
}
