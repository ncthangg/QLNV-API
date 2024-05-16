using System.ComponentModel.DataAnnotations.Schema;

namespace QLNV.Models.Entities
{
    public class JwtTokens
    {
        public int Id { get; set; }
        public string Token { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public DateTime Expires { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
