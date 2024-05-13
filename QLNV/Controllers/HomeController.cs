using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLNV.Models.DTOs;
using QLNV.Models.Entities;
using QLNV.Services;

namespace QLSV.Controllers
{
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IAuthenticationService _authenticationService;
        public HomeController(IAdminService adminService, IAuthenticationService authenticationService)
        {
            _adminService = adminService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("/api/[controller]/get-all-user")]
        public IEnumerable<UserDto> GetAllUser()
        {
            var users = _adminService.GetAllUser();
            return users;
        }

        [HttpGet]
        [Route("/api/[controller]/get-user/{id}")]
        public IActionResult GetUserByID(string id)
        {
            var usersDto = _adminService.GetUserById(id);
            if (usersDto != null)
            {
                return Ok(usersDto);
            }
            return BadRequest("Error!");

        }

        [HttpPost]
        [Route("/api/[controller]/create-user")]
        public IActionResult CreateUser([FromBody] UserDto userDto)
        {
            try
            {
                _adminService.AddUser(userDto);
                return Ok("User created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating user: {ex.Message}");
            }
        }
        [HttpDelete]
        [Route("/api/[controller]/delete-user/{id}")]
        public IActionResult DeleteUser(string id)
        {
            try
            {
                _adminService.DeleteUser(id);
                return Ok("Delete successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating user: {ex.Message}");
            }
        }
        [HttpPut]
        [Route("/api/[controller]/update-user/{id}")]
        public IActionResult UpdateUser(string id, [FromBody] UserDto newDto)
        {
            try
            {
                bool result = _adminService.UpdateUser(id, newDto);
                if (result)
                {
                    return Ok("Update successfully.");
                }
                else
                {
                    return BadRequest("UserID is not matched");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error updating user: UserID is not exist");
            }
        }

        [HttpGet]
        [Route("/api/[controller]/get-salary")]
        public IEnumerable<SalaryDto> GetSalary()
        {
            var salary = _adminService.GetSalary();
            return salary;
        }

        [HttpGet]
        [Route("/api/[controller]/get-salary/{id}")]
        public IActionResult GetSalaryById(string id)
        {
            try
            {
                var salaryDto = _adminService.GetSalaryById(id);
                if (salaryDto != null)
                {
                    return Ok(salaryDto);
                }
                return BadRequest("Error!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error search Salary's User: {ex.Message}");
            }

        }

        [HttpPost]
        [Route("/api/[controller]/add-salary")]
        public IActionResult AddSalary([FromBody] SalaryDto salaryDto)
        {
            try
            {
                _adminService.AddSalary(salaryDto);
                return Ok("Salary's User created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating user: {ex.Message}");
            }
        }
        [HttpPut]
        [Route("/api/[controller]/update-salary")]
        public IActionResult UpdateSalary(string userId, [FromBody] SalaryDto salaryDto)
        {
            try
            {
                _adminService.UpdateSalary(userId,salaryDto);
                return Ok("Salary's User updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating user: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("/api/[controller]/login")]
        public IActionResult Login([FromBody] RequestLoginDto requestLoginDto)
        {
            ResponseLogin responseLogin = _authenticationService.Login(requestLoginDto);

            if (responseLogin != null)
            {
               // _authenticationService.SaveToken(responseLogin);
                return Ok(responseLogin);
            }
            else
            {
                return Unauthorized("Invalid username or password");
            }
        }

    }
}
