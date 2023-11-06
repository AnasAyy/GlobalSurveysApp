using GlobalSurveysApp.Dtos.AdvanceDtos;
using GlobalSurveysApp.Dtos.TimeOffDtos;
using GlobalSurveysApp.Dtos.UserManagmentDtos.UserDtos;
using GlobalSurveysApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace GlobalSurveysApp.Data.Repo
{
    public interface ITimeOffRepo
    {
        public Task<int> CreateTimeOff(TimeOff timeOff);
        public Task CreateApprover(List<Approver> approver);
        public Task<User?> GetUserById(int id);
        public Task<string> GetFCM(int? id);
        public IQueryable<int> GetIdByRole(string type);
        public Task<IQueryable<GetAllTimeOffResponseDto>> GetAllTimeOffForUser(int id);
        public Task<IQueryable<GetAllTimeOffResponseDto>> GetTimeForUserByDate(int id, DateTime From, DateTime to);

        public TimeOff? GetTimeOffById(int id);

        public void UpdateTimeOff(TimeOff timeOff);

        public Task<List<Approver>> GetApproversByRequestId(int id);

        public Task<List<GetSubsituteEmployeeResponseDto>> GetSubsituteEmployee(int userId);
        public Task<List<Publics>> GetTypes();

        public Task<IQueryable<GetTimeOffForApproverResponseDto>> GetTimeOffForApprover(int id);

        public Task<IQueryable<GetTimeOffForApproverResponseDto>> GetTimeOffForApproverByDate(int id, DateTime From, DateTime to);

        public Task<IQueryable<GetTimeOffForApproverResponseDto>> GetTimeOffForApproverByName(int id, string name);
        public Task<Approver?> GetApprover(int requestId, int approvalId);
        public Task UpdateApprover(Approver approver);
        public Task<bool> SaveChanges();
        public Task<IEnumerable<object>> testGetFilteredUsersAsync(int userId);
        public Task<List<TimeOffsForSubEmpResponceDto>> GetTimeOffsForSubEmp(int SubEmp);
    }

    public class TimeOffs : ITimeOffRepo
    {
        private readonly DataContext _context;

        public TimeOffs(DataContext context)
        {
            _context = context;
        }

        public async Task CreateApprover(List<Approver> approver)
        {
            for (int i = 0; i < approver.Count; i++)
            {
                await _context.Approvers.AddAsync(approver[i]);
            }
        }

        public async Task<int> CreateTimeOff(TimeOff timeOff)
        {
            await _context.TimeOffs.AddAsync(timeOff);
            if (await SaveChanges())
                return timeOff.Id;
            return 0;
        }

        public async Task<User?> GetUserById(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
            _context.ChangeTracker.Clear();
            return user;
        }

        public async Task<string> GetFCM(int? id)
        {
            var token = await _context.FCMtokens
                .Where(x => x.UserId == id)
                .Select(x => x.Token)
                .SingleOrDefaultAsync() ?? string.Empty;

            return token;
        }

        public IQueryable<int> GetIdByRole(string type)
        {
            var user = from r in _context.Roles
                       join u in _context.Users on r.Id equals u.RoleId
                       where r.Title == type
                       select u.Id;

            return user;


        }

        public async Task<IQueryable<GetAllTimeOffResponseDto>> GetAllTimeOffForUser(int id)
        {
            var query = from timeOff in _context.TimeOffs
                        where timeOff.UserId == id
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

        public async Task<IQueryable<GetAllTimeOffResponseDto>> GetTimeForUserByDate(int id, DateTime From, DateTime to)
        {
            var query = from timeOff in _context.TimeOffs
                        where timeOff.UserId == id && timeOff.CreatedAt >= From && timeOff.CreatedAt <= to
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

        public TimeOff? GetTimeOffById(int id)
        {
            var timeOff = _context.TimeOffs.SingleOrDefault(x => x.Id == id);
            _context.ChangeTracker.Clear();
            return timeOff;
        }

        public void UpdateTimeOff(TimeOff timeOff)
        {

            _context.TimeOffs.Update(timeOff);

        }

        public async Task<List<Approver>> GetApproversByRequestId(int id)
        {
            var approvers = await _context.Approvers
                .Where(x => x.RequestId == id && x.RequestType == 2)
                .ToListAsync();

            return approvers;
        }

        public async Task<List<GetSubsituteEmployeeResponseDto>> GetSubsituteEmployee(int userId)
        {

            var substitutes = await _context.Users
            .Where(user => user.Id != userId)
            .Select(user => new GetSubsituteEmployeeResponseDto
            {
                Id = user.Id,
                Name = user.FirstName + " " + user.LastName
            })
              .ToListAsync();

            return substitutes;
        }

        public async Task<List<Publics>> GetTypes()
        {
            var types = await _context.PublicLists
                .Where(x => x.Type == 1031)
                .Select(x => new Publics
                {
                    Id = x.Id,
                    NameAR = x.NameAR,
                    NameEN = x.NameEN
                })
                .ToListAsync();

            return types;
        }

        public async Task<IQueryable<GetTimeOffForApproverResponseDto>> GetTimeOffForApprover(int id)
        {
            var query = from ap in _context.Approvers
                        join t in _context.TimeOffs on ap.RequestId equals t.Id
                        join u in _context.Users on t.UserId equals u.Id
                        where ap.UserId == id
                        where ap.RequestType == 2
                        where ap.CanViewed == true
                        select new GetTimeOffForApproverResponseDto
                        {
                            Id = t.Id,
                            Name = u.FirstName + " " + u.LastName,
                            Number = t.Number,
                            TypeAR = "أجازة",
                            TypeEN = "Time Off",
                            Date = t.CreatedAt
                        };

            return await Task.FromResult(query);
        }

        public async Task<IQueryable<GetTimeOffForApproverResponseDto>> GetTimeOffForApproverByDate(int id, DateTime From, DateTime to)
        {
            var query = from ap in _context.Approvers
                        join t in _context.TimeOffs on ap.RequestId equals t.Id
                        join u in _context.Users on t.UserId equals u.Id

                        where ap.UserId == id && t.CreatedAt >= From && t.CreatedAt <= to
                        where ap.RequestType == 2
                        where ap.CanViewed == true
                        select new GetTimeOffForApproverResponseDto
                        {
                            Id = t.Id,
                            Name = u.FirstName + " " + u.LastName,
                            Number = t.Number,
                            TypeAR = "أجازة",
                            TypeEN = "Time Off",
                            Date = t.CreatedAt
                        };
            return await Task.FromResult(query);
        }


        public async Task<IQueryable<GetTimeOffForApproverResponseDto>> GetTimeOffForApproverByName(int id, string name)
        {
            var query = from ap in _context.Approvers
                        join t in _context.TimeOffs on ap.RequestId equals t.Id
                        join u in _context.Users on t.UserId equals u.Id

                        where ap.UserId == id && (u.FirstName + " " + u.SecondName + " " + u.ThirdName + " " + u.LastName).Contains(name)
                        where ap.RequestType == 2
                        where ap.CanViewed == true
                        select new GetTimeOffForApproverResponseDto
                        {
                            Id = t.Id,
                            Name = u.FirstName + " " + u.LastName,
                            Number = t.Number,
                            TypeAR = "أجازة",
                            TypeEN = "Time Off",
                            Date = t.CreatedAt
                        };
            return await Task.FromResult(query);
        }

        public async Task<Approver?> GetApprover(int requestId, int approvalId)
        {


            var approver = await _context.Approvers
                .FirstOrDefaultAsync(x => x.RequestId == requestId && x.UserId == approvalId && x.RequestType == 2);
            _context.ChangeTracker.Clear();
            return approver;
        }

        public async Task UpdateApprover(Approver approver)
        {

            _context.Approvers.Update(approver);
            await _context.SaveChangesAsync();

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
        //////////////////////////////////////////////
        public async Task<IEnumerable<object>> testGetFilteredUsersAsync(int userId)
        {
            var result = await Task.Run(() =>
            {
                return (from u in _context.Users
                        where u.Id != userId && u.RoleId != 1 && u.IsActive == true &&
                              !(from t in _context.TimeOffs
                                where t.UserId == u.Id &&
                                      DateTime.Now.Date >= t.From.Date &&
                                      DateTime.Now.Date <= t.To.Date &&
                                      t.Status == RequestStatus.Accepted
                                select 1).Any()
                        select new
                        {
                            u.Id,
                            Name = u.FirstName + " " + u.LastName
                        }).ToList();
            });

            return result;
        }

        public async Task<List<TimeOffsForSubEmpResponceDto>> GetTimeOffsForSubEmp(int SubEmp)
        {

            var timeoffs = await (
            from timeOff in _context.TimeOffs
            join user in _context.Users on timeOff.UserId equals user.Id
            where timeOff.SubEmpStatus == RequestStatus.Pending && timeOff.SubstituteEmployeeId == SubEmp
            orderby timeOff.Id descending
            select new TimeOffsForSubEmpResponceDto
            {
                 Id = timeOff.Id,
                 Name = user.FirstName + " " + user.LastName,
                 From = timeOff.From,
                 Type = timeOff.Type,
                 Number = timeOff.Number
            }
            ).ToListAsync();

            return timeoffs;

        }


    }
}
