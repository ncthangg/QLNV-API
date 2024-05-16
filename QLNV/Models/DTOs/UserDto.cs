namespace QLNV.Models.DTOs
{
    public class UserDto : RegisterUserDto
    {

        public int JobId { get; set; }

        public int RoleId { get; set; }
    }
}
