using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNV.Models.Entities
{
    public class RefreshTokens
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public string? Token { get; set; }

        public DateTime? Expires { get; set; }

        public bool IsActive { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
