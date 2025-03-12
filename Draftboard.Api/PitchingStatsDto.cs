namespace Draftboard.Api
{
    public class PitchingStatsDto
    {
        public int Season { get; set; }
        public float InningsPitched { get; set; }
        public int StartingPitcherContribution { get; set; }
        public int Strikeouts { get; set; }
        public float ERA { get; set; }
        public float WHIP { get; set; }
        public int ReliefPitcherContribution { get; set; }
    }
}
