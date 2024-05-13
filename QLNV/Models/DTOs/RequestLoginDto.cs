using System.ComponentModel.DataAnnotations;

namespace QLNV.Models.DTOs
{
    public class RequestLoginDto
    {
        public string? UserId { get; set; }
        public string? Password { get; set; }
    }
}
