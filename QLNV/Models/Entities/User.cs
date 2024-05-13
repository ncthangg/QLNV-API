using System;
using System.Collections.Generic;

namespace QLNV.Models.Entities;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public DateOnly BirthDay { get; set; }

    public string Address { get; set; } = null!;

    public int JobId { get; set; }

    public int RoleId { get; set; }

    public string? VerificationToken { get; set; }

    public string? ResetToken { get; set; }

    public DateTime? ResetToenExpire { get; set; }

    public virtual JobPosition Job { get; set; } = null!;

    public virtual ICollection<RefreshTokens> RefreshTokens { get; set; } = new List<RefreshTokens>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    public virtual ICollection<UserRequest> UserRequests { get; set; } = new List<UserRequest>();

    public virtual ICollection<Claim> ClaimsClaims { get; set; } = new List<Claim>();
}
