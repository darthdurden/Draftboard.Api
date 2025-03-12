using Microsoft.EntityFrameworkCore;

namespace Draftboard.Api.Data.Models
{
    [Index("PlayerId")]
    [Index("PositionId")]
    public class PlayerPosition
    {
        public int Id { get; set; }
        public virtual Player Player { get; set; }
        public virtual Position Position { get; set; }
    }
}
