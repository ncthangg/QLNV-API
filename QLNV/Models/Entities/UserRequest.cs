using System;
using System.Collections.Generic;

namespace QLNV.Models.Entities;

public partial class UserRequest
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Reason { get; set; }

    public string? Attachment { get; set; }

    public DateTime? DayTime { get; set; }

    public virtual User User { get; set; } = null!;
}
