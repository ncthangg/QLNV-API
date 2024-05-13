using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QLNV.Models.Entities;

public partial class JobPosition
{
    [Key]
    public int JobId { get; set; }

    public string JobName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
