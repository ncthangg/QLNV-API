using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using QLNV.Models.DTOs;
using QLNV.Models.Entities;
using QLNV.Repositories;
using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Claim = System.Security.Claims.Claim;

namespace QLNV.Services
{
    public interface IAuthenticationService
    {
        ResponseLogin Login(RequestLoginDto requestLogin);
        string CreateJwtToken(User user);
        RefreshTokens CreateRefreshToken(User user);
        void Register(RegisterUserDto registerUserDto);
        bool VerifyOTP(string userId, string otp);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthenticationService(IUserRepository userRepository, IMapper mapper, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
            _mapper = mapper;
        }
        public class UserNotVerifiedException : Exception
        {
            public UserNotVerifiedException(string message) : base(message)
            {
            }
        }

        public ResponseLogin Login(RequestLoginDto requestLoginDto)
        {
            var account = _userRepository.GetUserByUserNameAndPassword(requestLoginDto.UserId, requestLoginDto.Password);
            
            if (account != null)
            {
                if (account.IsActive == true)
                {
                    //xóa token hết hạn
                    _userRepository.RemoveExpiredRefreshTokens();
                    _userRepository.RevokeOldTokens(requestLoginDto.UserId);

                    //tạo token và refresh token
                    var token = CreateJwtToken(account);
                    var refreshToken = CreateRefreshToken(account);
                    var result = new ResponseLogin
                    {
                        UserId = account.UserId,
                        RoleId = account.RoleId,
                        Token = token,
                        ResetToken = refreshToken.Token
                    };
                    User user = _userRepository.GetUserById(account.UserId);

                    var jwtTokenModel = new JwtTokens
                    {
                        UserId = account.UserId,
                        Token = token,
                        Expires = DateTime.Now.AddMinutes(30),
                    };
                    _userRepository.SaveResponsLogin(result);
                    _userRepository.SaveJwtToken(jwtTokenModel);
                    _userRepository.SaveRefreshToken(refreshToken);
                    return result;
                }
                else
                {
                    throw new UserNotVerifiedException("User chưa được xác thực!!!");
                }
            }
            throw new Exception("Thông tin đăng nhập không chính xác!");
        }

        public void Register(RegisterUserDto registerUserDto)
        {
            var userExist = _userRepository.GetUserById(registerUserDto.UserId);
            if (userExist == null)
            {
                var verifyToken = CreateOTP();
                //addDB
                var user = _mapper.Map<User>(registerUserDto);
                user.JobId = 2;
                user.RoleId = 2;
                user.VerificationToken = verifyToken;
                _userRepository.Add(user);

                SendOTP(registerUserDto.Email, verifyToken);
            }
            throw new Exception("UserId is existed");
        }
        private void SendOTP(string email, string verifyToken)
        {
            //sendEmail
            string fromAddress = _config["MailService:Mail"];
            string toAddress = email;
            string subject = "OTP code";
            string body = verifyToken;

            MailMessage message = new MailMessage(fromAddress, toAddress, subject, body);
            //var password = _config["Gmail:Password"];
            var password = _config["MailService:PasswordApp"];
            var smtp = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress, password),
                Timeout = 20000 // 20 seconds
            };
            smtp.Send(message);
        }

        private string CreateOTP()
        {
            string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string otp = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                string allowedChar = string.Empty;
                do
                {
                    int index = new Random().Next(0, allowedChars.Length);
                    allowedChar = allowedChars.ToCharArray()[index].ToString();
                } while (otp.IndexOf(allowedChar) != -1);
                otp += allowedChar;
            }
            return otp;
        }

        public bool VerifyOTP(string userId, string otp)
        {
            //ktra OTP dung?
            var user = _userRepository.GetUserById(userId);
            var verifyToken = user.VerificationToken.Trim();
            if (verifyToken == null)
            {
                return false;
            }
            else
            {
                bool isOTPValid = otp.Trim().Equals(verifyToken);
                if (isOTPValid)
                {
                    //update DB
                    user.IsActive = true;
                    _userRepository.Update(user);
                }
                return true;
            }
        }

        public ResponseLogin RefreshToken(string userId)
        {
            var user = _userRepository.GetUserById(userId);
            var tokenIsValid = _userRepository.IsTokenExpired(user.VerificationToken);
            if (!tokenIsValid)
            {
                var jwtToken = CreateJwtToken(user);
                var refreshToken = CreateRefreshToken(user);
                var result = new ResponseLogin
                {
                    UserId = user.UserId,
                    RoleId = user.RoleId,
                    Token = jwtToken,
                    ResetToken = refreshToken.Token
                };
                _userRepository.SaveResponsLogin(result);
                _userRepository.SaveRefreshToken(refreshToken);
                return result;
            }
            return null;
        }


        public string CreateJwtToken(User user)
        {
            var tokenHander = new JwtSecurityTokenHandler();
            //define Key
            var keyEncode = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var securityKey = new SymmetricSecurityKey(keyEncode);
            var creadential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //define description
            var tokenDescription = new SecurityTokenDescriptor
            {
                Audience = _config["Jwt:Issuer"],
                Issuer = _config["Jwt:Audience"],
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString())
                }),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = creadential
            };

            //define class Handler
            var token = tokenHander.CreateToken(tokenDescription);

            var tokenString = tokenHander.WriteToken(token);

            // Cập nhật token cũ thành không hoạt động
            _userRepository.DeactivateOldTokens(user.UserId);

            return tokenString;
        }
        public RefreshTokens CreateRefreshToken(User user)
        {
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var tokenHandler = new RNGCryptoServiceProvider();

            var randomByte = new byte[64];
            tokenHandler.GetBytes(randomByte);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte byteValue in randomByte)
            {
                stringBuilder.Append(allowedChars[byteValue % allowedChars.Length]);
            }
            var token = stringBuilder.ToString();
            // var token = Convert.ToBase64String(randomByte);

            var refreshToken = new RefreshTokens()
            {
                Token = token,
                UserId = user.UserId,
                Expires = DateTime.Now.AddDays(1),
                IsActive = true
            };
            return refreshToken;
        }
    }
}
