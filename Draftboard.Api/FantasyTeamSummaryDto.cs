namespace Draftboard.Api
{
    public class FantasyTeamSummaryDto
    {
        public string OwnerName { get; set; }
        public string Name { get; set; }
        public string TeamId { get; set; }
        public int DraftCashAdjustment { get; set; }
        public int SpentDraftCash { get; set; }
        public int RemainingDraftCash => 288 - SpentDraftCash + DraftCashAdjustment;
    }
}
