using Draftboard.Api.Data;
using Draftboard.Api.Data.Models;
using Draftboard.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Draftboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicksController : ControllerBase
    {
        private readonly DraftboardDbContext _context;

        public PicksController(DraftboardDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<IActionResult> GetPicks()
        {
            IQueryable<Player> filteredPlayers = _context.Players.Where(x => x.FantasyTeamPlayer != null && x.FantasyTeamPlayer.PickNumber.HasValue).OrderByDescending(x => x.FantasyTeamPlayer.PickNumber.Value);

            var players = await filteredPlayers.Select(x =>

                new PlayerDto
                {
                    Id = x.Id,
                    Age = x.Age,
                    MLBTeam = x.MLBTeam,
                    Name = x.Name,
                    Rank = x.Rank,
                    Contract = x.FantasyTeamPlayer.Contract,
                    PickNumber = x.FantasyTeamPlayer.PickNumber,
                    Salary = (int)x.FantasyTeamPlayer.Salary,
                    Positions = PlayersController.GetPositions(x.PlayerPositions.Select(y => y.Position.Id)),
                    HittingStats = x.PlayerStats.Where(x => x.Homeruns.HasValue).Select(y => new HittingStatsDto
                    {
                        Season = y.Season,
                        AtBats = y.AtBats ?? 0,
                        Homeruns = y.Homeruns.Value,
                        NetStolenBases2 = y.NetStolenBases2.Value,
                        OnBasePct = y.OnBasePct.Value,
                        RunsProduced = y.RunsProduced.Value,
                        TotalBases = y.TotalBases.Value
                    }),
                    PitchingStats = x.PlayerStats.Where(x => x.ERA.HasValue).Select(y => new PitchingStatsDto
                    {
                        Season = y.Season,
                        InningsPitched = y.InningsPitched ?? 0,
                        ERA = y.ERA.Value,
                        ReliefPitcherContribution = y.RPC.Value,
                        StartingPitcherContribution = y.SPC.Value,
                        Strikeouts = y.Strikeouts.Value,
                        WHIP = y.WHIP.Value
                    }),
                    FantasyTeam = x.FantasyTeamPlayer != null ? x.FantasyTeamPlayer.FantasyTeam.Id : null,
                    Headshot = $"https://fantraximg.com/si/headshots/MLB/hs{x.FantraxId.Trim('*')}_400_6.png",
                    FirstName = x.Name.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[0],
                    LastName = string.Join(' ', x.Name.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Skip(1)),
                }).ToListAsync();

            return Ok(players);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePick(int id)
        {
            var pick = await _context.FantasyTeamPlayers.FirstOrDefaultAsync(x => x.PickNumber == id);
            if(pick != null)
            {
                _context.Remove(pick);
                await _context.SaveChangesAsync();
            }

            return StatusCode(204);
        }

        [HttpPost]
        public async Task<IActionResult> PickAsync([FromBody]Pick pick)
        {
            var player = await _context.Players.Include(x => x.PlayerStats).FirstOrDefaultAsync(x => x.Id == pick.PlayerId);
            var lastPick = await _context.FantasyTeamPlayers.Where(x => x.PickNumber.HasValue).OrderBy(x => x.PickNumber).LastOrDefaultAsync();
            var nextPickNumber = lastPick != null ? lastPick.PickNumber + 1 : 1;

            var oldStats = player.PlayerStats.Where(x => x.Season < 2023).ToList();
            var ip = 0.0;
            var ab = 0;
            foreach (var stats in oldStats)
            {
                if(stats.InningsPitched.HasValue)
                {
                    var fullInnings = (int)Math.Floor(stats.InningsPitched.Value);
                    ip += fullInnings;

                    var extraOuts = (stats.InningsPitched.Value - fullInnings) * 10;

                    ip += stats.InningsPitched.Value + extraOuts/3.0;
                }


                if (stats.AtBats.HasValue)
                {
                    ab += stats.AtBats.Value;
                }
            }
            var contract = "MINOR";
            if(ip >= 50 || ab >= 130)
            {
                contract = "2025";
            }

            var team = await _context.FantasyTeams.FindAsync(pick.TeamId);

            await _context.AddAsync(new Data.Models.FantasyTeamPlayer
            {
                Contract = contract,
                Salary = pick.Bid,
                Player = player,
                FantasyTeam = team,
                PickNumber = nextPickNumber
            });

            await _context.SaveChangesAsync();

            return Ok(pick);
        }
    }
}
