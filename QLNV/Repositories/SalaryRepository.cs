using QLNV.CoreHelper;
using QLNV.Models.Entities;

namespace QLNV.Repositories
{
    public interface ISalaryRepository
    {
        IEnumerable<Salary> GetSalary();
        Salary? GetSalaryById(string userid);
        bool CheckMonthAndUser(string userId, int month);
        void AddSalary(Salary salary);
        void UpdateSalary(Salary salary);
        decimal CalculateTotalSalary(int contractSalary, int dayOff);
    }
    public class SalaryRepository : ISalaryRepository
    {
        
        private QuanLiNhanVienContext _context;

        public SalaryRepository(QuanLiNhanVienContext context)
        {
           _context = context;
        }

        public IEnumerable<Salary> GetSalary()
        {
            _context = new QuanLiNhanVienContext();
            return _context.Set<Salary>().ToList();
        }

        public Salary? GetSalaryById(string userId)
        {
            _context = new QuanLiNhanVienContext();
            var salary = _context.Set<Salary>().FirstOrDefault(x => x.UserId.ToLower() == userId.ToLower());


            if (salary != null)
            {
                return salary;
            }
            else
            {
                return null;   
            }
        }
        public bool CheckMonthAndUser(string userId, int month)
        {
            _context = new QuanLiNhanVienContext();
            return _context.Salary.Any(s => s.UserId == userId && s.Month == month);
        }

        public void AddSalary(Salary salary)
        {
            _context = new QuanLiNhanVienContext();
            _context.Set<Salary>().Add(salary);
            _context.SaveChanges();
        }
        public void UpdateSalary(Salary salary)
        {
            _context = new QuanLiNhanVienContext();
            _context.Set<Salary>().Update(salary);
            _context.SaveChanges();
        }
        public decimal CalculateTotalSalary(int contractSalary, int dayOff)
        {
            // Thực hiện tính toán TotalSalary dựa trên ContractSalary và DayOff
            decimal totalSalary = contractSalary - (contractSalary / 30.0m) * dayOff;
            return totalSalary;
        }

    }
}
