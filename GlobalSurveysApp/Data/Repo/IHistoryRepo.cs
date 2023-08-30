using GlobalSurveysApp.Dtos.AdvanceDtos;
using GlobalSurveysApp.Dtos.HistoryDtos;
using Microsoft.EntityFrameworkCore;

namespace GlobalSurveysApp.Data.Repo
{
    public interface IHistoryRepo
    {
        public Task<IQueryable<GetListOfUsersResponseDto>> GetListOfUsers();
        public Task<IQueryable<GetListOfUsersResponseDto>> GetListOfUsersByName(string name);
        public Task<TotalHistoryResponseDto> GetTotal(int userId);
        public Task<TotalHistoryResponseDto> GetTotalByDate(int userId, DateTime From, DateTime To);

    }

    public class History : IHistoryRepo
    {
        private readonly DataContext _context;

        public History(DataContext context)
        {
            _context = context;
        }

        

        public async Task<IQueryable<GetListOfUsersResponseDto>> GetListOfUsers()
        {
            var query = from user in _context.Users
                        where user.RoleId != 5
                        orderby user.Id descending
                        select new GetListOfUsersResponseDto
                        {
                            Id = user.Id,
                            Name = user.FirstName + " " + user.LastName

                        };

            return await Task.FromResult(query);
        }

        public async Task<IQueryable<GetListOfUsersResponseDto>> GetListOfUsersByName(string name)
        {
            var query = from user in _context.Users
                        where user.RoleId != 5 && (user.FirstName + " " + user.SecondName + " " + user.ThirdName + " " + user.LastName).Contains(name)
                        orderby user.Id descending
                        select new GetListOfUsersResponseDto
                        {
                            Id = user.Id,
                            Name = user.FirstName + " " + user.LastName
                        };

            return await Task.FromResult(query);
        }

        public async Task<TotalHistoryResponseDto> GetTotal(int userId)
        {
            var advance = await _context.Advances.CountAsync(a => a.UserId == userId);
            var timeOff = await _context.TimeOffs.CountAsync(t => t.UserId == userId);
            var complaint = await _context.Complaints.CountAsync(c => c.UserId == userId);
            return new TotalHistoryResponseDto
            {
                Advance = advance,
                TimeOff = timeOff,
                Complaint = complaint
            };
        }

        public async Task<TotalHistoryResponseDto> GetTotalByDate(int userId, DateTime From, DateTime To)
        {
            var advance = await _context.Advances.CountAsync(a => a.UserId == userId && a.CreateAt >= From && a.CreateAt <= To);
            var timeOff = await _context.TimeOffs.CountAsync(t => t.UserId == userId && t.CreatedAt >= From && t.CreatedAt <= To);
            var complaint = await _context.Complaints.CountAsync(c => c.UserId == userId && c.CreatedAt >= From && c.CreatedAt <= To);
            return new TotalHistoryResponseDto
            {
                Advance = advance,
                TimeOff = timeOff,
                Complaint = complaint
            };
           
        }
    }
}
