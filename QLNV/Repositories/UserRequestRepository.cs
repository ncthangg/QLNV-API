using QLNV.CoreHelper;
using QLNV.Models.Entities;

namespace QLNV.Repositories
{
    public interface IUserRequestRepository
    {
        void Add(UserRequest userRequest);
        //Task SaveAsync();
        List<UserRequest> GetAllRequest();
    }
    public class UserRequestRepository : IUserRequestRepository
    {

        private QuanLiNhanVienContext _context;

        public UserRequestRepository(QuanLiNhanVienContext context)
        {
            _context = context;
        }

        public void Add(UserRequest userRequest)
        {
            _context = new QuanLiNhanVienContext();
            _context.Set<UserRequest>().Add(userRequest);
            _context.SaveChanges();
        }
        //public async Task SaveAsync()
        //{
        //    await _context.SaveChangesAsync();
        //}
        public List<UserRequest> GetAllRequest()
        {
           _context = new QuanLiNhanVienContext();
           return  _context.Set<UserRequest>().ToList();
        }
    }
}
