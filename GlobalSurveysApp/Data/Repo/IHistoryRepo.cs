using GlobalSurveysApp.Dtos.AdvanceDtos;
using GlobalSurveysApp.Dtos.ComplaintDtos;
using GlobalSurveysApp.Dtos.HistoryDtos;
using GlobalSurveysApp.Dtos.TimeOffDtos;
using GlobalSurveysApp.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GlobalSurveysApp.Data.Repo
{
    public interface IHistoryRepo
    {
        public Task<IQueryable<GetListOfUsersResponseDto>> GetListOfUsers();
        public Task<IQueryable<GetListOfUsersResponseDto>> GetListOfUsersByName(string name);
        public Task<TotalHistoryResponseDto> GetTotal(int userId);
        public Task<TotalHistoryResponseDto> GetTotalByDate(int userId, DateTime From, DateTime To);
        public Task<IQueryable<GenaralFilterResponseDto>> GetUsers(int type, DateTime From, DateTime To);
        public Task<IQueryable<GetAllAdvanceResponseDto>> GetAdvanceForUserByDate(int id, DateTime From, DateTime to);
        public Task<IQueryable<GetAllTimeOffResponseDto>> GetTimeForUserByDate(int id, DateTime From, DateTime to);
        public Task<IQueryable<GetAllComplaintResponseDto>> GetComplaintForUserByDate(int id, DateTime From, DateTime to);
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
                        where user.RoleId != 5 && user.RoleId != 1
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
                        where user.RoleId != 5 && user.RoleId != 1 && (user.FirstName + " " + user.SecondName + " " + user.ThirdName + " " + user.LastName).Contains(name)
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

        public async Task<IQueryable<GenaralFilterResponseDto>> GetUsers(int type, DateTime From, DateTime To)
        {
            IQueryable<GenaralFilterResponseDto> query = null!;

            if (type == 1)
            {
                query = from u in _context.Users
                        join a in _context.Advances on u.Id equals a.UserId
                        where u.RoleId != 5 && u.RoleId != 1 && a.CreateAt >= From && a.CreateAt <= To && a.Status == RequestStatus.Accepted
                        group a by new { u.Id, u.FirstName, u.LastName } into g
                        select new GenaralFilterResponseDto
                        {
                            Id = g.Key.Id,
                            Name = g.Key.FirstName + " " + g.Key.LastName,
                            Count = g.Count()
                        };
            }
            else if (type == 2)
            {
                query = from u in _context.Users
                        join t in _context.TimeOffs on u.Id equals t.UserId
                        where u.RoleId != 5 && u.RoleId != 1 && t.CreatedAt >= From && t.CreatedAt <= To && t.Status == RequestStatus.Accepted
                        group t by new { u.Id, u.FirstName, u.LastName } into g
                        select new GenaralFilterResponseDto
                        {
                            Id = g.Key.Id,
                            Name = g.Key.FirstName + " " + g.Key.LastName,
                            Count = g.Count()
                        };
            }
            else if (type == 3)
            {
                query = from u in _context.Users
                        join c in _context.Complaints on u.Id equals c.UserId
                        where u.RoleId != 5 && u.RoleId != 1 && c.CreatedAt >= From && c.CreatedAt <= To && c.Status == RequestStatus.Accepted
                        group c by new { u.Id, u.FirstName, u.LastName } into g
                        select new GenaralFilterResponseDto
                        {
                            Id = g.Key.Id,
                            Name = g.Key.FirstName + " " + g.Key.LastName,
                            Count = g.Count()
                        };
            }

            return await Task.FromResult(query);
        }

        public async Task<IQueryable<GetAllAdvanceResponseDto>> GetAdvanceForUserByDate(int id, DateTime From, DateTime to)
        {
            var query = from advance in _context.Advances
                        where advance.UserId == id && advance.CreateAt >= From && advance.CreateAt <= to && advance.Status == RequestStatus.Accepted
                        orderby advance.Id descending
                        select new GetAllAdvanceResponseDto
                        {
                            Id = advance.Id,
                            Amount = advance.Amount,
                            CreateAt = advance.CreateAt,
                            Status = advance.Status,
                            IsUpdated = advance.IsUpdated
                        };

            return await Task.FromResult(query);
        }

        public async Task<IQueryable<GetAllTimeOffResponseDto>> GetTimeForUserByDate(int id, DateTime From, DateTime to)
        {
            var query = from timeOff in _context.TimeOffs
                        where timeOff.UserId == id && timeOff.CreatedAt >= From && timeOff.CreatedAt <= to && timeOff.Status == RequestStatus.Accepted
                        orderby timeOff.Id descending
                        select new GetAllTimeOffResponseDto
                        {
                            Id = timeOff.Id,
                            Type = timeOff.Type,
                            IsUpdated = timeOff.IsUpdated,
                            CreateAt = timeOff.CreatedAt,
                            Status = timeOff.Status,
                        };

            return await Task.FromResult(query);
        }

        public async Task<IQueryable<GetAllComplaintResponseDto>> GetComplaintForUserByDate(int id, DateTime From, DateTime to)
        {
            var query = from complaint in _context.Complaints
                        where complaint.UserId == id && complaint.CreatedAt >= From && complaint.CreatedAt <= to && complaint.Status == RequestStatus.Accepted
                        orderby complaint.Id descending
                        select new GetAllComplaintResponseDto
                        {
                            Id = complaint.Id,
                            Title = complaint.Title,
                            IsUpdated = complaint.IsUpdated,
                            CreateAt = complaint.CreatedAt,
                            Status = complaint.Status,
                        };

            return await Task.FromResult(query);
        }
    }
}
