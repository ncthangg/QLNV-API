
namespace QLNV.Models.DTOs
{
    public class SalaryDto
    {

        public string UserId { get; set; }

        public int Month { get; set; }

        public int ContractSalary { get; set; }

        public int DayOff { get; set; }

        //public decimal TotalSalary { get; set; }
    //ContractSalary - ((decimal)ContractSalary / 30 * DayOff);


    }
}
