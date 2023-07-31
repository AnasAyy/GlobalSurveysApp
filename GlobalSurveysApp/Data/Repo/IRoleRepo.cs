namespace GlobalSurveysApp.Data.Repo
{
    public interface IRoleRepo
    {
        public string GetRoleById(int id);
    }

    public class RoleRepo : IRoleRepo
    {
        private readonly DataContext _context;

        public RoleRepo(DataContext context)
        {
            _context = context;
        }

        public string GetRoleById(int id)
        {
            var role = _context.Roles.SingleOrDefault(x => x.Id == id);
            
            return role?.Title ?? "Unkowe Rolw" ;
        }
    }
}
