using System;
using System.Collections.Generic;

namespace QLNV.Models.Entities;

public partial class UserRequest
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string? Reason { get; set; }

    public string AttachmentPath { get; set; }
    public string AttachmentName { get; set; }
    public string AttachmentContentType { get; set; }

    public DateTime? DayTime { get; set; }

    public virtual User User { get; set; } = null!;
}
