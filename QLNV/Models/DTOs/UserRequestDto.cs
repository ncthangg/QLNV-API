namespace QLNV.Models.DTOs
{
    public class UserRequestDto
    {
        public string UserId { get; set; }
        public string? Reason { get; set; }
        public IFormFile Attachment { get; set; }
    }

}
