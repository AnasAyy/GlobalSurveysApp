﻿using GlobalSurveysApp.Dtos.AdvanceDtos;
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

        public Task<List<GetSubsituteEmployeeResponseDto>> GetSubsituteEmployee();
        public Task<List<Publics>> GetTypes();

        public Task<IQueryable<GetTimeOffForApproverResponseDto>> GetTimeOffForApprover(int id);

        public Task<IQueryable<GetTimeOffForApproverResponseDto>> GetTimeOffForApproverByDate(int id, DateTime From, DateTime to);

        public  Task<IQueryable<GetTimeOffForApproverResponseDto>> GetTimeOffForApproverByName(int id, string name);
        public Task<bool> SaveChanges();
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
                        orderby timeOff.CreatedAt descending
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
                        orderby timeOff.CreatedAt descending
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

        public async Task<List<GetSubsituteEmployeeResponseDto>> GetSubsituteEmployee()
        {
            var users = await _context.Users.ToListAsync();

            var substitutes = users.Select(user => new GetSubsituteEmployeeResponseDto
            {
                Id = user.Id,
                Name = user.FirstName + " " + user.LastName
            }).ToList();

            return substitutes;
        }

        public async Task<List<Publics>> GetTypes()
        {
            return await Task.Run(() =>
            {
                return _context.PublicLists
                    .Where(x => x.Type == 1031)
                    .Select(x => new Publics
                    {
                        Id = x.Id,
                        NameAR = x.NameAR,
                        NameEN = x.NameEN
                    })
                    .ToList();
            });
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
