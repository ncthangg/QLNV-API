using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QLNV.Models.DTOs;
using QLNV.Models.Entities;

namespace QLNV.CoreHelper;

public partial class QuanLiNhanVienContext : DbContext
{
    public QuanLiNhanVienContext()
    {
    }

    public QuanLiNhanVienContext(DbContextOptions<QuanLiNhanVienContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Claim> Claims { get; set; }

    public virtual DbSet<JobPosition> JobPosition { get; set; }

    public virtual DbSet<Role> Role { get; set; }

    public virtual DbSet<Salary> Salary { get; set; }

    public virtual DbSet<User> User { get; set; }

    public virtual DbSet<UserRequest> UserRequests { get; set; }

    public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }

    public virtual DbSet<ResponseLogin> ResponseLogin { get; set; }
    public virtual DbSet<JwtTokens> JwtTokens { get; set; }


    private string? GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:DefaultConnectionStringDB"];
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());


}
