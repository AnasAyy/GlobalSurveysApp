using GlobalSurveysApp.Dtos.PublicListDtos;
using GlobalSurveysApp.Dtos.UserManagmentDtos.UserDtos;
using GlobalSurveysApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSurveysApp.Data.Repo
{
    public interface IUserRepo
    {
        public void Create(User user);
        public void Update(User user);
        public User? GetUserById(int id);
        public  Task<User?> GetUserByIdAsync(int id);
        
        public IQueryable<GetAllUSersResponseDto> GetUserByName(string name);
        public IQueryable<GetAllUSersResponseDto> GetUserByNameActive(string name);
        public IQueryable<GetAllUSersResponseDto> GetUserByNameDis_Active(string name);
        public IQueryable<GetAllUSersResponseDto> GetUserByType(bool type);
        public IQueryable<GetAllUSersResponseDto> GetAllUsers();
        public bool IsExits(string privateMobalie);
        public IQueryable<ViewUserResponceDto> GetUserDetails(int id);
        public GetPublicListResponceDto GetPublicList();
        public Task<List<Role>> GetRole();
        public IQueryable<GetDirectResponsibleResponseDto> GetDirectResponsible();

        public bool SaveChanges();
    }

    public class UserRepo : IUserRepo
    {
        private readonly DataContext _context;

        public UserRepo(DataContext context)
        {
            _context = context;
        }

        public void Create(User user)
        {
            _context.Users.Add(user);
        }


        public IQueryable<GetAllUSersResponseDto> GetAllUsers()
        {
            return from u in _context.Users
                   orderby u.Id descending
                   select new GetAllUSersResponseDto
                   {
                       Id = u.Id,
                       Name = u.FirstName + " " + u.LastName,
                       PhoneNumber = u.PrivateMobile,
                       IsActive = u.IsActive,
                   };
        }


        public GetPublicListResponceDto GetPublicList()
        {
            var P = new GetPublicListResponceDto();

            P.PlaceOfBirth = GetPublicsListByType(1014);
            P.CertificateLevel = GetPublicsListByType(1009);
            P.FieldOfStudy = GetPublicsListByType(1010);
            P.Gender = GetPublicsListByType(1008);
            P.Postion = GetPublicsListByType(1011);
            P.Nationality = GetPublicsListByType(1012);
            P.Department = GetPublicsListByType(1013);
            P.Location = GetPublicsListByType(1007);

            return P;
        }

        private List<Publics> GetPublicsListByType(int type)
        {
            return _context.PublicLists
                .Where(x => x.Type == type)
                .Select(x => new Publics
                {
                    Id = x.Id,
                    NameAR = x.NameAR,
                    NameEN = x.NameEN
                })
                .ToList();
        }

        public User? GetUserById(int id)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == id);
            _context.ChangeTracker.Clear();
            return user;
        }

        public IQueryable<GetAllUSersResponseDto> GetUserByName(string name)
        {
            return from u in _context.Users
                   where (u.FirstName + " " + u.SecondName + " " + u.ThirdName + " " + u.LastName).Contains(name)
                   select new GetAllUSersResponseDto
                   {
                       Id = u.Id,
                       Name = u.FirstName + " " + u.LastName,
                       PhoneNumber = u.PrivateMobile,
                       IsActive = u.IsActive,
                   };
        }

        public IQueryable<GetAllUSersResponseDto> GetUserByType(bool type)
        {
            return from u in _context.Users
                   where u.IsActive == type
                   select new GetAllUSersResponseDto
                   {
                       Id = u.Id,
                       Name = u.FirstName + " " + u.LastName,
                       PhoneNumber = u.PrivateMobile,
                       IsActive = u.IsActive,
                   };
        }



        public IQueryable<ViewUserResponceDto> GetUserDetails(int id)
        {
            var query = from u in _context.Users
                        join r in _context.Roles on u.RoleId equals r.Id
                        where u.Id == id
                        select new ViewUserResponceDto
                        {
                            Id = u.Id,
                            FirstName = u.FirstName,
                            SecondName = u.SecondName,
                            ThirdName = u.ThirdName,
                            LastName = u.LastName,
                            WorkMobile = u.WorkMobile,
                            PrivateMobile = u.PrivateMobile,
                            PlaceOfBirth = u.placeOfBirth,
                            DateOfBirth = u.DateOfBirth,
                            CertificateLevel = u.CertificateLevel,
                            FieldOfStudy = u.FieldOfStudy,
                            PassportNumber = u.PassportNumber,
                            Gender = u.Gender,
                            FirstContractDate = u.FirstContractDate,
                            Position = u.Postion,
                            Nationality = u.Nationality,
                            Email = u.Email,
                            Department = u.Department,
                            Location = u.Location,
                            DirectResponsibleId = u.DirectResponsibleId,
                            Status = u.IsActive,
                            Role = r.Title,
                            CreatedAt = u.CreatedAt,
                            LastUpdatedAt = u.UpdatedAt,
                            LastLogIn = u.LastLogin,
                        };

            return query;
        }




        public bool IsExits(string privateMobalie)
        {
            return _context.Users.Any(x => x.PrivateMobile == privateMobalie);
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

        public void Update(User user)
        {

            _context.Users.Update(user);
        }

        public IQueryable<GetDirectResponsibleResponseDto> GetDirectResponsible()
        {
            return from r in _context.Roles
                   join u in _context.Users on r.Id equals u.RoleId
                   where r.Title == "Direct responsible"
                   select new GetDirectResponsibleResponseDto {
                       Id = u.Id,
                       Name = u.FirstName + " " + u.LastName
                   };
        }

        
        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
            _context.ChangeTracker.Clear();
            return user;
        }

        public async Task<List<Role>> GetRole()
        {
            return await _context.Roles.ToListAsync();
        }

        public IQueryable<GetAllUSersResponseDto> GetUserByNameActive(string name)
        {
            return from u in _context.Users
                   where (u.FirstName + " " + u.SecondName + " " + u.ThirdName + " " + u.LastName).Contains(name)
                   where u.IsActive == true
                   select new GetAllUSersResponseDto
                   {
                       Id = u.Id,
                       Name = u.FirstName + " " + u.LastName,
                       PhoneNumber = u.PrivateMobile,
                       IsActive = u.IsActive,
                   };
        }

        public IQueryable<GetAllUSersResponseDto> GetUserByNameDis_Active(string name)
        {
            return from u in _context.Users
                   where (u.FirstName + " " + u.SecondName + " " + u.ThirdName + " " + u.LastName).Contains(name)
                   where u.IsActive == false
                   select new GetAllUSersResponseDto
                   {
                       Id = u.Id,
                       Name = u.FirstName + " " + u.LastName,
                       PhoneNumber = u.PrivateMobile,
                       IsActive = u.IsActive,
                   };
        }
    }

}
