using System.Collections.Generic;

namespace Draftboard.Api.Data.Models
{
    public class FantasyTeam
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public int DraftCashAdjustment { get; set; }
        public virtual ICollection<FantasyTeamPlayer> FantasyTeamPlayers { get; set; }
    }
}
