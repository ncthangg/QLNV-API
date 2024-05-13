using System;
using System.Collections.Generic;

namespace QLNV.Models.Entities;

public partial class Claim
{
    public int ClaimId { get; set; }

    public string ClaimName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
