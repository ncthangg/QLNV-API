using QLNV.CoreHelper;

namespace QLNV.Repositories
{
    public interface IUserRequestRepository
    {

    }
    public class UserRequestRepository : IUserRequestRepository
    {

        private QuanLiNhanVienContext _context;

        public UserRequestRepository(QuanLiNhanVienContext context)
        {
            _context = context;
        }
    }
}
