using GlobalSurveysApp.Dtos.UserManagmentDtos.LoginManagement;
using GlobalSurveysApp.Models;


namespace GlobalSurveysApp.Data.Repo
{
    public interface ILoginRepo 
    {
        public User? LoginViaUserName(LoginViaUsernameRequestDto request);
        public User? LoginViaQRcode(LoginViaQRcodeRequestDto request);
        public void Update(User user);
        public User? GetUserById(int id);
        public bool SaveChanges();
        public bool IsVerified(int id);
        public void CreateFCM(FCMtoken fCMtoken);
        public void DeleteFCM(int Id);
        
        
    }

    public class LoginRepo : ILoginRepo
    {
        private readonly DataContext _context;

        public LoginRepo(DataContext context)
        {
            _context = context;
        }

        public User? LoginViaUserName(LoginViaUsernameRequestDto request)
        {
            var result = _context.Users.SingleOrDefault(x => x.UserName == request.Username && x.Password == request.Password);
            if (result != null)
            {
                return result;
            }
            return null;
        }

        public User? LoginViaQRcode(LoginViaQRcodeRequestDto request)
        {
            var result = _context.Users.SingleOrDefault(x => x.QRcode == request.QRcode);
            if (result != null)
            {
                return result;
            }
            return null;
        }

        public bool IsUserExist(LoginViaUsernameRequestDto request)
        {
            return _context.Users.Any(x => x.UserName == request.Username && x.Password == request.Password);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public User? GetUserById(int id)
        {
            return _context.Users.SingleOrDefault(x => x.Id == id);
        }

        public bool SaveChanges()
        {
            try
            {
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                _context.ChangeTracker.Clear();
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public bool IsVerified(int id)
        {

            var result = _context.Users.SingleOrDefault(_x => _x.Id == id);

            return result != null ? result.IsVerified : false;

        }

        public void CreateFCM(FCMtoken fCMtoken)
        {
            _context.FCMtokens.Add(fCMtoken);
        }

        public void DeleteFCM(int Id)
        {
            var tokenToDelete = _context.FCMtokens.Where(x => x.UserId == Id);
            if (tokenToDelete.Any())
            {
                _context.FCMtokens.RemoveRange(tokenToDelete);
            }
            
        }
    }
}
