using Draftboard.Api.Data;
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
    public class FantasyTeamsController : ControllerBase
    {
        private readonly DraftboardDbContext _context;

        public FantasyTeamsController(DraftboardDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<IActionResult> GetSummariesAsync()
        {
            var summaries = await _context.FantasyTeams.Select(x => new FantasyTeamSummaryDto
            {
               Name = x.Name,
               OwnerName = x.OwnerName,
               TeamId = x.Id,
               DraftCashAdjustment = x.DraftCashAdjustment,
               SpentDraftCash = (int)x.FantasyTeamPlayers.Sum(y => y.Salary)
            }).ToListAsync();
            return Ok(summaries);
        }

        [HttpGet("{teamId}/roster")]
        public async Task<IActionResult> GetRosterAsync(string teamId)
        {
            var team = await _context.FantasyTeams
                    .Include(x => x.FantasyTeamPlayers).ThenInclude(x => x.Player).ThenInclude(x => x.PlayerStats)
                    .Include(x=> x.FantasyTeamPlayers).ThenInclude(x => x.Player).ThenInclude(x => x.PlayerPositions).ThenInclude(x => x.Position)
                    .FirstOrDefaultAsync(x => x.Id == teamId);

            var players = new List<PlayerDto>();

            foreach (var ftp in team.FantasyTeamPlayers) {
                var player = ftp.Player;

                players.Add(
                new PlayerDto
                {
                    Id = player.Id,
                    Age = player.Age,
                    MLBTeam = player.MLBTeam,
                    Name = player.Name,
                    Rank = player.Rank,
                    Positions = GetPositions(player.PlayerPositions.Select(y => y.Position.Id)),
                    Contract = player.FantasyTeamPlayer.Contract,
                    Salary = (int)player.FantasyTeamPlayer.Salary,
                    FantasyTeam = player.FantasyTeamPlayer.FantasyTeam.Id,
                    PickNumber = player.FantasyTeamPlayer.PickNumber,
                    HittingStats = player.PlayerStats.Where(x => x.Homeruns.HasValue).Select(y => new HittingStatsDto
                    {
                        Season = y.Season,
                        AtBats = y.AtBats ?? 0,
                        Homeruns = y.Homeruns.Value,
                        NetStolenBases2 = y.NetStolenBases2.Value,
                        OnBasePct = y.OnBasePct.Value,
                        RunsProduced = y.RunsProduced.Value,
                        TotalBases = y.TotalBases.Value
                    }),
                    PitchingStats = player.PlayerStats.Where(x => x.ERA.HasValue).Select(y => new PitchingStatsDto
                    {
                        Season = y.Season,
                        InningsPitched = y.InningsPitched ?? 0,
                        ERA = y.ERA.Value,
                        ReliefPitcherContribution = y.RPC.Value,
                        StartingPitcherContribution = y.SPC.Value,
                        Strikeouts = y.Strikeouts.Value,
                        WHIP = y.WHIP.Value
                    }),
                    Headshot = $"https://fantraximg.com/si/headshots/MLB/hs{player.FantraxId.Trim('*')}_400_6.png",
                    FirstName = player.Name.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[0],
                    LastName = string.Join(' ', player.Name.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Skip(1)),
                });
            }

            return Ok(players);
        }

        private static IEnumerable<string> GetPositions(IEnumerable<string> positions)
        {
            var offensivePositions = new string[] { "C", "1B", "2B", "SS", "3B", "OF" };
            var pitchingPositions = new string[] { "SP", "RP" };
            var positionsHash = new HashSet<string>(positions);
            if (positionsHash.Any(x => offensivePositions.Contains(x)))
            {
                positionsHash.Add("UT");
            }

            if (positionsHash.Any(x => pitchingPositions.Contains(x)))
            {
                positionsHash.Add("P");
            }

            return positionsHash;
        }
    }
}
