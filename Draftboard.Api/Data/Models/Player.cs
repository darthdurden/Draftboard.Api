using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Draftboard.Api.Data.Models
{
    [Index("Rank")]
    public class Player
    {
        public int Id { get; set; }
        public string FantraxId { get; set; }
        public string Name { get; set; }
        public string MLBTeam { get; set; }
        public int Rank { get; set; }
        public int Age { get; set; }
        public virtual ICollection<PlayerStats> PlayerStats { get; set; }
        public virtual ICollection<PlayerPosition> PlayerPositions { get; set; }
        public virtual FantasyTeamPlayer FantasyTeamPlayer { get; set; }
    }
}
