using Microsoft.EntityFrameworkCore;
using QLNV.CoreHelper;
using QLNV.Models.Entities;
using QLNV.Services;

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
        void SaveToken(ResponseLogin responLogin);
        void SaveRefreshToken(RefreshTokens refreshTokens);
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
            return _context.User.FirstOrDefault(u => u.UserId == userId && u.Password == password);
        }

        public void SaveToken(ResponseLogin responLogin)
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
    }
}
