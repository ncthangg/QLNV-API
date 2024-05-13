using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNV.Models.Entities
{
    public class ResponseLogin
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; }
        public string ResetToken { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public int? RoleId { get; set; }

        public virtual User User { get; set; } = null!;

    }
}
