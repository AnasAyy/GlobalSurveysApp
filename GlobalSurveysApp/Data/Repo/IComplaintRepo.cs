using Azure.Core;
using GlobalSurveysApp.Dtos.ComplaintDtos;
using GlobalSurveysApp.Dtos.TimeOffDtos;
using GlobalSurveysApp.Dtos.UserManagmentDtos.UserDtos;
using GlobalSurveysApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace GlobalSurveysApp.Data.Repo
{
    public interface IComplaintRepo
    {
        public Task<int> CreateComplaint(Complaint complaint);
        public Task CreateApprover(Approver approver);
        public IQueryable<int> GetIdByRole(string type);
        public Task<User?> GetUserById(int id);
        public Task<string> GetFCM(int? id);
        public Task<IQueryable<GetAllComplaintResponseDto>> GetAllComplaintForUser(int id);
        public Task<IQueryable<GetAllComplaintResponseDto>> GetComplaintForUserByDate(int id, DateTime From, DateTime to);
        public Complaint? GetComplaintById(int id);

        public void UpdateComplaint(Complaint complaint);
        public Task<Approver?> GetLastApproverByRequestId(int id);
        public Task<List<Publics>> GetTitles();
        public Task<IQueryable<GetComplaintForApproverResponseDto>> GetComplaintForApprover(int id);
        public Task<IQueryable<GetComplaintForApproverResponseDto>> GetComplaintForApproverByDate(int id, DateTime From, DateTime to);

        public Task<IQueryable<GetComplaintForApproverResponseDto>> GetComplaintForApproverByName(int id, string name);
        public Task<Approver?> GetApprover(int requestId, int approvalId);
        public Task UpdateApprover(Approver approver);

        public void Complaint_BS();
        public Task<bool> SaveChanges();
    }

    public class ComplaintRepo : IComplaintRepo
    {
        private readonly DataContext _context;

        public ComplaintRepo(DataContext context)
        {
            _context = context;
        }

        public async Task CreateApprover(Approver approver)
        {
            await _context.Approvers.AddAsync(approver);
        }

        public async Task<int> CreateComplaint(Complaint complaint)
        {
            await _context.Complaints.AddAsync(complaint);
            if (await SaveChanges())
                return complaint.Id;
            return 0;
        }

        public IQueryable<int> GetIdByRole(string type)
        {
            var user = from r in _context.Roles
                       join u in _context.Users on r.Id equals u.RoleId
                       where r.Title == type
                       select u.Id;

            return user;


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

        public async Task<IQueryable<GetAllComplaintResponseDto>> GetAllComplaintForUser(int id)
        {
            var query = from complaint in _context.Complaints
                        where complaint.UserId == id
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

        public async Task<IQueryable<GetAllComplaintResponseDto>> GetComplaintForUserByDate(int id, DateTime From, DateTime to)
        {
            var query = from complaint in _context.Complaints
                        where complaint.UserId == id && complaint.CreatedAt >= From && complaint.CreatedAt <= to
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

        public Complaint? GetComplaintById(int id)
        {
            var complaint = _context.Complaints.SingleOrDefault(x => x.Id == id);
            _context.ChangeTracker.Clear();
            return complaint;
        }

        public void UpdateComplaint(Complaint complaint)
        {

            _context.Complaints.Update(complaint);

        }

        public async Task<Approver?> GetLastApproverByRequestId(int id)
        {
            var lastApprover = await _context.Approvers
                .Where(x => x.RequestId == id && x.RequestType == 3)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            return lastApprover;
        }

        public async Task<List<Publics>> GetTitles()
        {
            var types = await _context.PublicLists
                .Where(x => x.Type == 1033)
                .Select(x => new Publics
                {
                    Id = x.Id,
                    NameAR = x.NameAR,
                    NameEN = x.NameEN
                })
                .ToListAsync();

            return types;
        }

        public async Task<IQueryable<GetComplaintForApproverResponseDto>> GetComplaintForApprover(int id)
        {
            var query = from ap in _context.Approvers
                        join c in _context.Complaints on ap.RequestId equals c.Id
                        join u in _context.Users on c.UserId equals u.Id
                        where ap.UserId == id
                        where ap.RequestType == 3
                        where ap.CanViewed == true
                        orderby c.Id descending
                        select new GetComplaintForApproverResponseDto
                        {
                            Id = c.Id,
                            Name = u.FirstName + " " + u.LastName,
                            Title = c.Title,
                            TypeAR = "شكوى",
                            TypeEN = "Complaint",
                            Date = c.CreatedAt
                        };

            return await Task.FromResult(query);
        }

        public async Task<IQueryable<GetComplaintForApproverResponseDto>> GetComplaintForApproverByName(int id, string name)
        {
            var query = from ap in _context.Approvers
                        join c in _context.Complaints on ap.RequestId equals c.Id
                        join u in _context.Users on c.UserId equals u.Id

                        where ap.UserId == id && (u.FirstName + " " + u.SecondName + " " + u.ThirdName + " " + u.LastName).Contains(name)
                        where ap.RequestType == 3
                        where ap.CanViewed == true
                        orderby c.Id descending
                        select new GetComplaintForApproverResponseDto
                        {
                            Id = c.Id,
                            Name = u.FirstName + " " + u.LastName,
                            Title = c.Title,
                            TypeAR = "شكوى",
                            TypeEN = "Complaint",
                            Date = c.CreatedAt
                        };
            return await Task.FromResult(query);
        }

        public async Task<IQueryable<GetComplaintForApproverResponseDto>> GetComplaintForApproverByDate(int id, DateTime From, DateTime to)
        {
            var query = from ap in _context.Approvers
                        join c in _context.Complaints on ap.RequestId equals c.Id
                        join u in _context.Users on c.UserId equals u.Id

                        where ap.UserId == id && c.CreatedAt >= From && c.CreatedAt <= to
                        where ap.RequestType == 3
                        where ap.CanViewed == true
                        orderby c.Id descending
                        select new GetComplaintForApproverResponseDto
                        {
                            Id = c.Id,
                            Name = u.FirstName + " " + u.LastName,
                            Title = c.Title,
                            TypeAR = "شكوى",
                            TypeEN = "Complaint",
                            Date = c.CreatedAt
                        };
            return await Task.FromResult(query);
        }

        public async Task<Approver?> GetApprover(int requestId, int approvalId)
        {
            var approver = await _context.Approvers
                .FirstOrDefaultAsync(x => x.RequestId == requestId && x.UserId == approvalId && x.RequestType == 3);
            _context.ChangeTracker.Clear();
            return approver;
        }
        public async Task UpdateApprover(Approver approver)
        {
            _context.Approvers.Update(approver);
            await _context.SaveChangesAsync();
        }
        public void Complaint_BS()
        {

            try
            {
                


                    var twoDaysAgo = DateTime.Now.AddDays(-2);

                    var query = from c in _context.Complaints
                                join a in _context.Approvers on c.Id equals a.RequestId
                                where a.RequestType == 3 && a.Status == 0 && c.CreatedAt < twoDaysAgo && a.ApproverType == 2
                                select new
                                {
                                    c.Id,
                                    a.UserId,
                                };

                    var complaints =  query.ToList();

                    foreach (var complaint in complaints)
                    {
                        var approverP =  _context.Approvers
                            .FirstOrDefault(x => x.RequestId == complaint.Id && x.UserId == complaint.UserId && x.RequestType == 3);

                        if (approverP != null)
                        {
                            approverP.CanViewed = false;
                            approverP.Status = RequestStatus.Rejected;
                            approverP.Note = "Moved";
                            _context.Approvers.Update(approverP);

                        var managerId = GetIdByRole("Manager").FirstOrDefault();
                        Approver newApprover = new Approver
                        {
                            ApproverType = 3,
                            CanViewed = true,
                            RequestId = approverP.RequestId,
                            RequestType = 3,
                            Status = RequestStatus.Pending,
                            UserId = managerId
                        };

                        _context.Approvers.Add(newApprover);

                    }
                }
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


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


    }
}
