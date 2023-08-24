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
        public Task<IQueryable<GetMessagesForTellerResponseDto>> GetMessagesForTeller(int userId);
        public Message? GetMessageById(int id);
        public void UpdateMessage(Message message);
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

        public async Task<IQueryable<GetMessagesForTellerResponseDto>> GetMessagesForTeller(int userId)
        {
            var query = from m in _context.Messages
                        join p in _context.PublicLists on m.Type equals p.Id
                        where m.UserId == userId
                        select new GetMessagesForTellerResponseDto
                        {
                            Id= m.Id,
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
    }
}
