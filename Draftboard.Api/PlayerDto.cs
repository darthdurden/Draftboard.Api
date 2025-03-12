using System.Collections.Generic;

namespace Draftboard.Api
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MLBTeam { get; set; }
        public string Headshot { get; set; }
        public int Rank { get; set; }
        public int Age { get; set; }
        public string Contract { get; set; }
        public int? Salary { get; set; }
        public int? PickNumber { get; set; }
        public string FantasyTeam { get; set; }
        public IEnumerable<string> Positions { get; set; }
        public IEnumerable<HittingStatsDto> HittingStats { get; set; }
        public IEnumerable<PitchingStatsDto> PitchingStats { get; set; }
    }
}
