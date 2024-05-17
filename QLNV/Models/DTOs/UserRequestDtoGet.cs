namespace QLNV.Models.DTOs
{
    public class UserRequestDtoGet : UserRequestDto
    {
        public string UserId { get; set; }
        public string? Reason { get; set; }
        public string AttachmentName { get; set; }
        public DateTime? DayTime { get; set; }
    }

}
