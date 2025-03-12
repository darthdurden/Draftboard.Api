using Microsoft.EntityFrameworkCore;

namespace Draftboard.Api.Data.Models
{
    [Index(nameof(Season))]
    public class PlayerStats
    {
        public int Id { get; set; }
        public virtual Player Player { get; set; }
        public int Season { get; set; }


        public int? AtBats { get; set; }
        public int? Homeruns { get; set; }
        public float? NetStolenBases2 { get; set; }
        public float? OnBasePct { get; set; }
        public int? TotalBases { get; set; }
        public int? RunsProduced { get; set; }


        public float? InningsPitched { get; set; }
        public int? SPC { get; set; }
        public int? Strikeouts { get; set; }
        public float? ERA { get; set; }
        public float? WHIP { get; set; }
        public int? RPC { get; set; }
    }
}
