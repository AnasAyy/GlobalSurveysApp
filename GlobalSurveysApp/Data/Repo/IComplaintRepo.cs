using GlobalSurveysApp.Dtos.ComplaintDtos;
using GlobalSurveysApp.Dtos.TimeOffDtos;
using GlobalSurveysApp.Models;
using Microsoft.EntityFrameworkCore;

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
                        orderby complaint.CreatedAt descending
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
