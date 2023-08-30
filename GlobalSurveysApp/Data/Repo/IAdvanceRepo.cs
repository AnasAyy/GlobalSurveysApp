using GlobalSurveysApp.Dtos.AdvanceDtos;
using GlobalSurveysApp.Dtos.UserManagmentDtos.UserDtos;
using GlobalSurveysApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSurveysApp.Data.Repo
{
    public interface IAdvanceRepo
    {
        public Task<int> CreateAdvance(Advance advance);
        public  Task CreateApprover(List<Approver> approver);
        public void  Update(Advance advance);
        public Task UpdateApprover(Approver approver);
        public Task<IQueryable<GetAllAdvanceResponseDto>> GetAllAdvanceForUser(int id);
        public Task<IQueryable<GetAllAdvanceResponseDto>> GetAdvanceForUserByDate(int id, DateTime From, DateTime to);
        public Task<List<Approver>> GetApproversByRequestId(int id);
        public Task<Approver?> GetApprover(int requestId, int approvalId);
        public  Advance? GetAdvanceById(int id);
        public Task<User?> GetUserById(int id);
        public Task<Advance?> GetAdvance(int id);
        public IQueryable<int> GetIdByRole(string type);
        public Task<string> GetFCM(int? id);
        public Task<List<Publics>> GetCurrency();
        public Task<IQueryable<GetAdvanceForApproverResponseDto>> GetAdvanceForApprover(int id);
        public Task<IQueryable<GetAdvanceForApproverResponseDto>> GetAdvanceForApproverByDate(int id, DateTime From, DateTime to);
        public Task<IQueryable<GetAdvanceForApproverResponseDto>> GetAdvanceForApproverByName(int id, string name);
        public Task<bool> SaveChanges();
    }

    public class AdvanceRepo : IAdvanceRepo
    {
        private readonly DataContext _context;

        public AdvanceRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<int>  CreateAdvance(Advance advance)
        {
            await _context.Advances.AddAsync(advance);
            if(await SaveChanges())
            return advance.Id;
            return 0;
        }

        public async Task CreateApprover(List<Approver> approver)
        {
            for(int i = 0; i < approver.Count; i++)
            {
                await _context.Approvers.AddAsync(approver[i]);
            }
        }



        public async Task<IQueryable<GetAllAdvanceResponseDto>> GetAdvanceForUserByDate(int id, DateTime From, DateTime to)
        {
            var query = from advance in _context.Advances
                        where advance.UserId == id && advance.CreateAt >= From && advance.CreateAt <= to
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

        public async Task<IQueryable<GetAllAdvanceResponseDto>> GetAllAdvanceForUser(int id)
        {
            var query = from advance in _context.Advances
                        where advance.UserId == id
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

        public async Task<User?> GetUserById(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
            _context.ChangeTracker.Clear();
            return user;
        }

        public IQueryable<int> GetIdByRole(string type)
        {
            var user = from r in _context.Roles
                        join u in _context.Users on r.Id equals u.RoleId
                        where r.Title == type
                        select u.Id;

            return user;


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

        public void Update(Advance advance)
        {
             _context.Advances.Update(advance);
        }
        public async Task UpdateApprover(Approver approver)
        {
            
                _context.Approvers.Update(approver);
                await _context.SaveChangesAsync();
            
        }

        public async Task<string> GetFCM(int? id)
        {
            var token = await _context.FCMtokens
                .Where(x => x.UserId == id)
                .Select(x => x.Token)
                .SingleOrDefaultAsync() ?? string.Empty;

            return token;
        }

        public Advance? GetAdvanceById(int id)
        {
            var advance = _context.Advances.SingleOrDefault(x => x.Id == id);
            _context.ChangeTracker.Clear();
            return advance;
        }

        public async Task<List<Approver>> GetApproversByRequestId(int id)
        {
            var approvers = await _context.Approvers
                .Where(x => x.RequestId == id && x.RequestType == 1 )
                .ToListAsync();

            return approvers;
        }

        public async Task<List<Publics>> GetCurrency()
        {
            var currencies = await _context.PublicLists
                .Where(x => x.Type == 1028)
                .Select(x => new Publics
                {
                    Id = x.Id,
                    NameAR = x.NameAR,
                    NameEN = x.NameEN
                })
                .ToListAsync();

            return currencies;
        }

        public async Task<IQueryable<GetAdvanceForApproverResponseDto>> GetAdvanceForApprover(int id)
        {
            var query = from ap in _context.Approvers
                        join a in _context.Advances on ap.RequestId equals a.Id
                        join u in _context.Users on a.UserId equals u.Id
                        where ap.UserId == id
                        where ap.RequestType == 1
                        where ap.CanViewed == true
                        select new GetAdvanceForApproverResponseDto
                        {
                            Id = a.Id,
                            Name = u.FirstName + " " + u.LastName,
                            Amount = a.Amount,
                            TypeAR = "سلفة",
                            TypeEN = "Advance", 
                            Date = a.CreateAt
                        };

            return await Task.FromResult(query);
        }
        public async Task<IQueryable<GetAdvanceForApproverResponseDto>> GetAdvanceForApproverByDate(int id, DateTime From, DateTime to)
        {
            var query = from ap in _context.Approvers
                        join a in _context.Advances on ap.RequestId equals a.Id
                        join u in _context.Users on a.UserId equals u.Id
                        
                        where ap.UserId == id && a.CreateAt >= From && a.CreateAt <= to
                        where ap.RequestType == 1
                        where ap.CanViewed == true
                        select new GetAdvanceForApproverResponseDto
                        {
                            Id = a.Id,
                            Name = u.FirstName + " " + u.LastName,
                            Amount = a.Amount,
                            TypeAR = "سلفة",
                            TypeEN = "Advance",
                            Date = a.CreateAt
                        };

            return await Task.FromResult(query);
        }

        public async Task<IQueryable<GetAdvanceForApproverResponseDto>> GetAdvanceForApproverByName(int id, string name)
        {
            var query = from ap in _context.Approvers
                        join a in _context.Advances on ap.RequestId equals a.Id
                        join u in _context.Users on a.UserId equals u.Id
                        where ap.UserId == id &&  (u.FirstName + " " + u.SecondName + " " + u.ThirdName + " " + u.LastName).Contains(name)
                        where ap.RequestType == 1
                        where ap.CanViewed == true
                        select new GetAdvanceForApproverResponseDto
                        {
                            Id = a.Id,
                            Name = u.FirstName + " " + u.LastName,
                            Amount = a.Amount,
                            TypeAR = "سلفة",
                            TypeEN = "Advance",
                            Date = a.CreateAt
                        };

            return await Task.FromResult(query);
        }

        public async Task<Approver?> GetApprover(int requestId, int approvalId)
        {
            var approver = await _context.Approvers
                .FirstOrDefaultAsync(x => x.RequestId == requestId && x.UserId == approvalId && x.RequestType == 1);
            _context.ChangeTracker.Clear();
            return approver;
        }

        public async Task<Advance?> GetAdvance(int id)
        {
            return await _context.Advances.SingleOrDefaultAsync(x => x.Id == id);
            
        }
        
    }
}
