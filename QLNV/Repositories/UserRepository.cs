using Microsoft.EntityFrameworkCore;
using QLNV.CoreHelper;
using QLNV.Models.Entities;
using QLNV.Services;
using System.IdentityModel.Tokens.Jwt;

namespace QLNV.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUser();
        User? GetUserById(string id);
        void Add(User user);
        void Update(User user);
        void Delete(string userId);
        User GetUserByUserNameAndPassword(string userId, string password);
        ResponseLogin GetUserFromToken(string token);
        void SaveResponsLogin(ResponseLogin responLogin);
        void SaveRefreshToken(RefreshTokens refreshTokens);
        void SaveJwtToken(JwtTokens jwtTokens);
        void RemoveExpiredRefreshTokens();
        void RevokeOldTokens(string userId, int maxTokens = 5);
        bool IsTokenExpired(string token);
        bool IsTokenActive(string token);
        void DeactivateOldTokens(string userId);
    }
    public class UserRepository : IUserRepository
    {

        private QuanLiNhanVienContext _context;

        public UserRepository(QuanLiNhanVienContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUser()
        {
            _context = new QuanLiNhanVienContext();
            return _context.Set<User>().ToList();
        }

        public User? GetUserById(string userId)
        {
            _context = new QuanLiNhanVienContext();
             return _context.Set<User>().FirstOrDefault(x => x.UserId.ToLower() == userId.ToLower());
        }

        public void Add(User user)
        {
            _context = new QuanLiNhanVienContext();
            _context.Set<User>().Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context = new QuanLiNhanVienContext();
            _context.Set<User>().Update(user);
            _context.SaveChanges();
        }

        public void Delete(string userId)
        {
            _context = new QuanLiNhanVienContext();
            var user = _context.Set<User>().FirstOrDefault(x => x.UserId.ToLower() == userId.ToLower());
            if (user != null)
            {
                _context.Set<User>().Remove(user);
                _context.SaveChanges();
            }
        }

        public bool IsUserIdExists(string userId)
        {
            return _context.Set<User>().Any(user => user.UserId == userId);
        }

        public bool IsEmailExists(string email)
        {
            return _context.Set<User>().Any(user => user.Email == email);
        }

        public User GetUserByUserNameAndPassword(string userId, string password)
        {
            return _context.Set<User>().FirstOrDefault(u => u.UserId == userId && u.Password == password);
        }

       public ResponseLogin GetUserFromToken(string token)
        {
            _context = new QuanLiNhanVienContext();
            return _context.Set<ResponseLogin>().FirstOrDefault(x => x.Token == token);
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////// Authen & Author Repository
        /// </summary>
        /// <param name="responLogin"></param>

        public void SaveResponsLogin(ResponseLogin responLogin)
        {
            _context = new QuanLiNhanVienContext();
            _context.Set<ResponseLogin>().Add(responLogin);
            _context.SaveChanges();
        }
        public void SaveRefreshToken(RefreshTokens refreshTokens)
        {
            _context = new QuanLiNhanVienContext();
            _context.Set<RefreshTokens>().Add(refreshTokens);
            _context.SaveChanges();
        }
        public void SaveJwtToken(JwtTokens jwtTokens)
        {
            _context = new QuanLiNhanVienContext();
            _context.Set<JwtTokens>().Add(jwtTokens);
            _context.SaveChanges();
        }
        public bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var expirationDate = jwtToken.ValidTo;

            // Check if the token is expired
            return expirationDate < DateTime.Now;
        }
        public bool IsTokenActive(string token)
        {
            var tokenEntry = _context.JwtTokens.FirstOrDefault(rt => rt.Token == token);
            return tokenEntry != null && tokenEntry.IsActive;
        }
        public void DeactivateOldTokens(string userId)
        {
            var tokens = _context.JwtTokens.Where(t => t.UserId == userId && t.IsActive).ToList();
            if (tokens.Any())
            {
                foreach (var token in tokens)
                {
                    token.IsActive = false;
                }
                _context.SaveChanges();
            }
        }

        public void RemoveExpiredRefreshTokens()
        {
            try
            {
                var allTokens = _context.RefreshTokens.ToList();
                Console.WriteLine("All tokens:");
                foreach (var token in allTokens)
                {
                    Console.WriteLine($"Token ID: {token.Id}, Expires: {token.Expires}");
                }

                var expiredTokens = _context.RefreshTokens.Where(rt => rt.Expires < DateTime.Now).ToList();
                Console.WriteLine($"Found {expiredTokens.Count} expired tokens.");

                if (expiredTokens.Any())
                {
                    _context.RefreshTokens.RemoveRange(expiredTokens);
                    _context.SaveChanges();
                    Console.WriteLine("Expired tokens removed.");
                }
                else
                {
                    Console.WriteLine("No expired tokens to remove.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing expired tokens: {ex.Message}");
                // Optionally rethrow or handle the exception as needed
            }
        }
        public void RevokeOldTokens(string userId, int maxTokens = 5)
        {
            var userTokens = _context.RefreshTokens.Where(rt => rt.UserId == userId).OrderByDescending(rt => rt.Expires).ToList();
            if (userTokens.Count > maxTokens)
            {
                var tokensToRevoke = userTokens.Skip(maxTokens).ToList();
                _context.RefreshTokens.RemoveRange(tokensToRevoke);
                _context.SaveChanges();
            }
        }
    }
}
