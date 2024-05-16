namespace QLNV.Models.DTOs
{
    public class UserRequestDto
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Attachment { get; set; }
    }

}
