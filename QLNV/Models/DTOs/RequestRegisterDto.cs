namespace QLNV.Models.DTOs
{
    public class RequestRegisterDto
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Sex { get; set; } = null!;
        public DateOnly BirthDay { get; set; }
        public string Address { get; set; } = null!;
        public int JobId { get; set; }
        public int RoleId { get; set; }


    }
}
