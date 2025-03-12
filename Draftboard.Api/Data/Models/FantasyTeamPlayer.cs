using System.ComponentModel.DataAnnotations.Schema;

namespace Draftboard.Api.Data.Models
{
    public class FantasyTeamPlayer
    {
        public int Id { get; set; }
        [ForeignKey("PlayerId")]
        public Player Player { get; set; }
        public FantasyTeam FantasyTeam { get; set; }
        public string Contract { get; set; }
        public float Salary { get; set; }
        public int? PickNumber { get; set; }
    }
}
