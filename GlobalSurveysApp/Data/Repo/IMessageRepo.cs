using GlobalSurveysApp.Dtos.MessageDtos;
using GlobalSurveysApp.Dtos.UserManagmentDtos.UserDtos;
using GlobalSurveysApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSurveysApp.Data.Repo
{
    public interface IMessageRepo
    {
        public Task<List<Publics>> GeTDepartments();
        public Task<List<GetAllUSersInMessageResponseDto>> GetAllUsers(int userId);

        public Task CreateMessage(Message message);

        public Task<List<Publics>> GetMessageType();
        public Task<List<Publics>> GetGroups();
        public Task<IQueryable<GetMessagesResponseDto>> GetMessagesForTeller(int userId);
        public Message? GetMessageById(int id);
        public void UpdateMessage(Message message);
        public Task<GetMessageDetalisResponceDtos> ViewMessagesDetalisforTeller(int id);
        public Task<IQueryable<GetMessagesResponseDto>> GetMessagesForReciver(int userId);
        public Task<GetMessageDetalisForReciverResponceDtos> ViewMessagesDetalisforReciver(int userId, int messagId);
        public Task<bool> SaveChanges();


    }


    public class Messages : IMessageRepo
    {

        private readonly DataContext _context;

        public Messages(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Publics>> GeTDepartments()
        {
            var department = await _context.PublicLists
                .Where(x => x.Type == 1013)
                .Select(x => new Publics
                {
                    Id = x.Id,
                    NameAR = x.NameAR,
                    NameEN = x.NameEN
                })
                .ToListAsync();

            return department;
        }

        public async Task<List<GetAllUSersInMessageResponseDto>> GetAllUsers(int userId)
        {
            return await (from u in _context.Users
                          where u.Id != userId
                          select new GetAllUSersInMessageResponseDto
                          {
                              Id = u.Id,
                              Name = u.FirstName + " " + u.LastName
                          }).ToListAsync();
        }
        public async Task<List<Publics>> GetMessageType()
        {
            var department = await _context.PublicLists
                .Where(x => x.Type == 1035)
                .Select(x => new Publics
                {
                    Id = x.Id,
                    NameAR = x.NameAR,
                    NameEN = x.NameEN
                })
                .ToListAsync();

            return department;
        }

        public async Task<List<Publics>> GetGroups()
        {
            var department = await _context.PublicLists
                .Where(x => x.Type == 1014)
                .Select(x => new Publics
                {
                    Id = x.Id,
                    NameAR = x.NameAR,
                    NameEN = x.NameEN
                })
                .ToListAsync();

            return department;
        }
        public async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _context.ChangeTracker.Clear();
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public async Task CreateMessage(Message message)
        {
            await _context.Messages.AddAsync(message);
        }

        public async Task<IQueryable<GetMessagesResponseDto>> GetMessagesForTeller(int userId)
        {
            var query = from m in _context.Messages
                        join p in _context.PublicLists on m.Type equals p.Id
                        where m.UserId == userId
                        orderby m.Id descending
                        select new GetMessagesResponseDto
                        {
                            Id = m.Id,
                            Title = m.Title,
                            Date = m.CreatedAt,
                            TypeAR = p.NameAR,
                            TypeEN = p.NameEN,
                        };
            return await Task.FromResult(query);
        }

        public Message? GetMessageById(int id)
        {
            var message = _context.Messages.SingleOrDefault(x => x.Id == id);
            _context.ChangeTracker.Clear();
            return message;
        }

        public void UpdateMessage(Message message)
        {
            _context.Messages.Update(message);
        }

        public async Task<GetMessageDetalisResponceDtos> ViewMessagesDetalisforTeller(int id)
        {
            var message = await _context.Messages.SingleOrDefaultAsync(x => x.Id == id);
            if (message != null)
            {
                string ToWhomeEN = "To All";
                string ToWhomeAR = "الى الجميع";
                if (message.Type == 1037)
                {
                    var dep = await _context.PublicLists.SingleOrDefaultAsync(y => y.Id == message.ToWhom);
                    if (dep != null)
                    {
                        ToWhomeAR =  message.ToWhom.ToString();
                        ToWhomeEN =  message.ToWhom.ToString();
                    }

                }
                if (message.Type == 1038)
                {
                    var user = await _context.Users.SingleOrDefaultAsync(y => y.Id == message.ToWhom);
                    if (user != null)
                    {
                        ToWhomeAR = message.ToWhom.ToString();
                        ToWhomeEN = message.ToWhom.ToString();
                    }

                }
                if (message.Type == 1087)
                {
                    var user = await _context.PublicLists.SingleOrDefaultAsync(y => y.Id == message.ToWhom);
                    if (user != null)
                    {
                        ToWhomeAR = message.ToWhom.ToString();
                        ToWhomeEN = message.ToWhom.ToString();
                    }

                }

                return new GetMessageDetalisResponceDtos
                {
                    Title = message.Title,
                    Body = message.Body,
                    Date = message.CreatedAt,
                    Type = message.Type,
                    ToWhomAR = ToWhomeAR,
                    ToWhomEN = ToWhomeEN,
                };
            }
            return null!;
        }

        public async Task<IQueryable<GetMessagesResponseDto>> GetMessagesForReciver(int userId)
        {
            var query = from message in _context.Messages
                        join publicList in _context.PublicLists on message.Type equals publicList.Id
                        where message.UserId != userId
                        where message.Type == 1036 ||
                              (message.Type == 1038 && message.ToWhom == userId)  
                        select new GetMessagesResponseDto
                        {
                            Id = message.Id,
                            Title = message.Title,
                            Date = message.CreatedAt,
                            TypeAR = publicList.NameAR,
                            TypeEN = publicList.NameEN
                        };
            var query1 = from message in _context.Messages
                         join publicList in _context.PublicLists on message.Type equals publicList.Id
                         join user in _context.Users on message.ToWhom equals user.Department
                         where message.Type == 1037
                         where message.UserId != userId
                         where user.Id == userId
                         select new GetMessagesResponseDto
                         {
                             Id = message.Id,
                             Title = message.Title,
                             Date = message.CreatedAt,
                             TypeAR = publicList.NameAR,
                             TypeEN = publicList.NameEN
                         };
            var combinedQuery = query.Union(query1).OrderByDescending(message => message.Id); ;
            return await Task.FromResult(combinedQuery);
        }

        public async Task<GetMessageDetalisForReciverResponceDtos> ViewMessagesDetalisforReciver(int userId, int messagId)
        {
            
            var message = await _context.Messages.SingleOrDefaultAsync(x => x.Id == messagId);
            if (message != null)
            {
                string ToWhomeEN = "To All";
                string ToWhomeAR = "الى الجميع";
                if (message.Type == 1037)
                {
                    var dep = await _context.PublicLists.SingleOrDefaultAsync(y => y.Id == message.ToWhom);
                    if (dep != null)
                    {
                        ToWhomeAR = dep.NameAR;
                        ToWhomeEN = dep.NameEN;
                    }

                }
                if (message.Type == 1038)
                {
                    var user = await _context.Users.SingleOrDefaultAsync(y => y.Id == message.ToWhom);
                    if (user != null)
                    {
                        ToWhomeAR = user.FirstName + " " + user.LastName;
                        ToWhomeEN = user.FirstName + " " + user.LastName;
                    }

                }
                var roleId = await _context.Users
            .Where(x => x.Id == message.UserId)
            .Select(x => x.RoleId)
            .SingleOrDefaultAsync();

                var userRole = await _context.Roles
                .Where(x => x.Id == roleId)
                .Select(x => x.Title)
                .SingleOrDefaultAsync();
                if (userRole == null)
                {
                    return null!;
                }
                return new GetMessageDetalisForReciverResponceDtos
                {
                    Title = message.Title,
                    Body = message.Body,
                    From = userRole,
                    Date = message.CreatedAt,
                    Type = message.Type,
                    ToWhomAR = ToWhomeAR,
                    ToWhomEN = ToWhomeEN,
                };
            }
            return null!;


        }
    }
}
