using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLNV.Models.DTOs;
using QLNV.Models.Entities;
using QLNV.Services;
using System.Security.Claims;
using static QLNV.Services.AuthenticationService;

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
        [Authorize(AuthenticationSchemes = "Bearer", Roles ="1")]
        [Route("/api/[controller]/Accounts/get-all-user")]
        public IEnumerable<UserDto> GetAllUser()
        {
                var users = _adminService.GetAllUser();
                return users;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("/api/[controller]/Accounts/get-user/{id}")]
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "1")]
        [Route("/api/[controller]/Accounts/create-user")]
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "1")]
        [Route("/api/[controller]/Accounts/delete-user/{id}")]
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "1")]
        [Route("/api/[controller]/Accounts/update-user/{id}")]
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "1")]
        [Route("/api/[controller]/Accounts/get-salary")]
        public IEnumerable<SalaryDto> GetSalary()
        {
            var salary = _adminService.GetSalary();
            return salary;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "2")]
        [Route("/api/[controller]/Accounts/get-salary/{id}")]
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "1")]
        [Route("/api/[controller]/Accounts/add-salary")]
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "1")]
        [Route("/api/[controller]/Accounts/update-salary")]
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
            try
            {
                ResponseLogin responseLogin = _authenticationService.Login(requestLoginDto);

                if (responseLogin != null)
                {
                    // _authenticationService.SaveToken(responseLogin);
                    return Ok(responseLogin);
                }
                //else
                //{
                //    return Unauthorized("Invalid username or password");
                //}
            }
            catch (UserNotVerifiedException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }
            return StatusCode(500, $"Error logging user");
        }

        [HttpPost]
        [Route("/api/[controller]/register")]
        public IActionResult Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                _authenticationService.Register(registerUserDto);
                return Ok("User created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating user: {ex.Message}");
            }
        }


        [HttpPost]
        [Route("/api/[controller]/verify")]
        public IActionResult VerifyOTP([FromBody] VerifyDto verifyDto)
        {
            try
            {
                var isOTPValid = _authenticationService.VerifyOTP(verifyDto.UserId, verifyDto.VerificationToken);
                if (isOTPValid)
                {
                    return Ok("OTP is valid");
                }
                else
                {
                    return BadRequest("Invalid OTP");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating user: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("/api/[controller]/verify")]
        public IActionResult SendRequest([FromBody] UserRequestDto userRequestDto)
        {
            try
            {
                
                if (isOTPValid)
                {
                    return Ok("OTP is valid");
                }
                else
                {
                    return BadRequest("Invalid OTP");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating user: {ex.Message}");
            }
        }
    }
}
