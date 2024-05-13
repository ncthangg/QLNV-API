using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNV.Models.Entities;

public partial class Salary
{
    public int Id { get; set; }
    [ForeignKey("UserId")]
    public string UserId { get; set; } = null!;

    public int Month { get; set; }

    public int ContractSalary { get; set; }

    public int DayOff { get; set; }

    public decimal TotalSalary { get; set; }

    public virtual User User { get; set; } = null!;
}
