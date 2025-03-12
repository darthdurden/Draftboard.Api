namespace Draftboard.Api
{
    public class HittingStatsDto
    {
        public int Season { get; set; }
        public int AtBats { get; set; }
        public int Homeruns { get; set; }
        public float NetStolenBases2 { get; set; }
        public float OnBasePct { get; set; }
        public int TotalBases { get; set; }
        public int RunsProduced { get; set; }
    }
}
