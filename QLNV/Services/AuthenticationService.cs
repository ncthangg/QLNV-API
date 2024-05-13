using Microsoft.IdentityModel.Tokens;
using QLNV.Models.DTOs;
using QLNV.Models.Entities;
using QLNV.Repositories;
using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Claim = System.Security.Claims.Claim;

namespace QLNV.Services
{
    public interface IAuthenticationService
    {
        ResponseLogin Login(RequestLoginDto requestLogin);
        void SaveToken(ResponseLogin responseLogin);

    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly string Key = "sadasdasjdacbjasdnajscnajacacajjcs";

        private readonly IUserRepository _userRepository;
        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ResponseLogin Login(RequestLoginDto requestLoginDto)
        {
            var account = _userRepository.GetUserByUserNameAndPassword(requestLoginDto.UserId, requestLoginDto.Password);
            if (account != null)
            {
                var token = CreateJwtToken(account);
                var refreshToken = CreateRefreshToken(account);
                var result = new ResponseLogin
                {
                    UserId = account.UserId,
                    RoleId = account.RoleId,
                    Token = token,
                    ResetToken = refreshToken.Token
                };
                SaveToken(result);
                return result;

            }
            return null;
        }
        public void SaveToken(ResponseLogin responseLogin)
        {
            _userRepository.SaveToken(responseLogin);
        }

        private string CreateJwtToken(User user)
        {
            var tokenHander = new JwtSecurityTokenHandler();
            //define Key
            var keyEncode = Encoding.UTF8.GetBytes(Key);
            var securityKey = new SymmetricSecurityKey(keyEncode);
            var creadential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //define description
            var tokenDescription = new SecurityTokenDescriptor
            {
                Audience = "",
                Issuer = "",
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("RoleID", user.RoleId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = creadential
            };

            //define class Handler
            var token = tokenHander.CreateToken(tokenDescription);

            var tokenString = tokenHander.WriteToken(token);

            //var responseLogin = new ResponseLogin
            //{
            //    Token = tokenString,
            //    UserId = user.UserId,
            //    RoleId = user.RoleId // Đặt vai trò cho người dùng mới đăng ký
            //};

            return tokenString;
        }
        private RefreshTokens CreateRefreshToken(User user)
        {
            var randomByte = new byte[64];
            //var tokenHandler = new RNGCryptoServiceProvider();
            var token = Convert.ToBase64String(randomByte);
            var refreshToken = new RefreshTokens()
            {
                Token = token,
                UserId = user.UserId,
                Expires = DateTime.UtcNow.AddDays(1),
                IsActive = true
            };
            _userRepository.SaveRefreshToken(refreshToken);
            return refreshToken;
        }
    }
}
