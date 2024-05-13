using AutoMapper;
using QLNV.Models.DTOs;
using QLNV.Models.Entities;
using QLNV.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QLNV.Services
{
    public interface IAdminService
    {
        IEnumerable<UserDto> GetAllUser();
        UserDto? GetUserById(string userId);
        bool AddUser(UserDto userDto);
        bool UpdateUser(string userId, UserDto dto);
        bool DeleteUser(string userId);
        IEnumerable<SalaryDto> GetSalary();
        SalaryDto? GetSalaryById(string userId);
        void AddSalary(SalaryDto salaryDto);
        void UpdateSalary(string userId,SalaryDto salaryDto);
    }


    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISalaryRepository _salaryRepository;
        private readonly IMapper _mapper;
        public AdminService(IUserRepository userRepository, IMapper mapper, ISalaryRepository salaryRepository)
        {
            _userRepository = userRepository;
            _salaryRepository = salaryRepository;
            _mapper = mapper;
        }


        public IEnumerable<UserDto> GetAllUser()
        {
            IEnumerable<User> users = _userRepository.GetAllUser();
            IEnumerable<UserDto> userDto = _mapper.Map<IEnumerable<UserDto>>(users);
            return userDto;
        }
        public UserDto? GetUserById(string userId)
        {
            User user = _userRepository.GetUserById(userId);
            if (user != null)
            {
                UserDto userDto = _mapper.Map<UserDto>(user);
                return userDto;
            }
            throw new Exception("UserId is not exist");

        }

        public bool AddUser(UserDto userDto)
        {

            User user = _userRepository.GetUserById(userDto.UserId);
            if (user == null)
            {
                user = _mapper.Map<User>(userDto);
                _userRepository.Add(user);
                return true;
            }
            throw new Exception("User already exists.");

        }
        public bool DeleteUser(string userId)
        {
            var existUser = _userRepository.GetUserById(userId);
            if (existUser != null)
            {
                _userRepository.Delete(userId);
                return true;
            }
            return false;

        }
        public bool UpdateUser(string userId, UserDto dto)
        {
            try
            {
                User existUser = _userRepository.GetUserById(userId);
                if (existUser == null)
                {
                    return false;
                }
                if (!existUser.UserId.Equals(userId))
                {
                    return false;
                }

                User newUser = _mapper.Map(dto, existUser);
                _userRepository.Update(existUser);
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Error updating user: " + ex.Message);
            }
        }
        //========================================================
        public IEnumerable<SalaryDto> GetSalary()
        {
            IEnumerable<Salary> salary = _salaryRepository.GetSalary();
            IEnumerable<SalaryDto> salaryDto = _mapper.Map<IEnumerable<SalaryDto>>(salary);
            return salaryDto;
        }

        public SalaryDto? GetSalaryById(string userId)
        {
            User user = _userRepository.GetUserById(userId);
            if (user != null || user.UserId == userId)
            {
                Salary salary = _salaryRepository.GetSalaryById(userId);
                if (salary != null)
                {
                    SalaryDto salaryDto = _mapper.Map<SalaryDto>(salary);
                    return salaryDto;
                }
            }
            throw new Exception("UserId is not exist");

        }
        public void AddSalary(SalaryDto salaryDto)
        {
            decimal totalSalary = _salaryRepository.CalculateTotalSalary(salaryDto.ContractSalary, salaryDto.DayOff);
            User existUser = _userRepository.GetUserById(salaryDto.UserId);
            if (existUser != null)
            {
                // Tạo đối tượng Salary từ DTO
                var salary = new Salary
                {
                    UserId = salaryDto.UserId,
                    Month = salaryDto.Month,
                    ContractSalary = salaryDto.ContractSalary,
                    DayOff = salaryDto.DayOff,
                    TotalSalary = totalSalary  // Gán giá trị TotalSalary đã tính toán
                };
                _salaryRepository.AddSalary(salary);
            }
            else
            {
                throw new Exception("UserId is not exist");
            }
        }

        public void UpdateSalary(string userId, SalaryDto salaryDto)
        {
            decimal totalSalary = _salaryRepository.CalculateTotalSalary(salaryDto.ContractSalary, salaryDto.DayOff);
            User existUser = _userRepository.GetUserById(salaryDto.UserId);

            if (existUser != null)
            {
                Salary exitSalary = _salaryRepository.GetSalaryById(userId);
                if (exitSalary != null)
                {
                    var newSalary = _mapper.Map(salaryDto, exitSalary);
                    newSalary.TotalSalary = totalSalary;
                    _salaryRepository.UpdateSalary(newSalary);
                }
            }
            else
            {
                throw new Exception("UserId is not exist");
            }
        }


        //========================================================


    }
}
