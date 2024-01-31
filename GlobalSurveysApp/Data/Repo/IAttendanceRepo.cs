using GlobalSurveysApp.Dtos.AttendanceDtos.AttendanceDto;
using GlobalSurveysApp.Dtos.AttendanceDtos.LocationDto;
using GlobalSurveysApp.Dtos.AttendanceDtos.WorkingHourDto;
using GlobalSurveysApp.Dtos.PublicListDtos;
using GlobalSurveysApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSurveysApp.Data.Repo
{
    public interface IAttendanceRepo
    {
        public Task CreateWorkingHour(WorkingHour workingHour);
        public void UpdateWorkingHour(WorkingHour workingHour);
        public Task<bool> GetWorkingHourForAdd(AddWorkingHourRequestDto addWorkingHourDto);
        public Task<bool> GetWoorkingHourByParForUpdate(UpdateWorkingHourRequestDto updateWorkingHourDto);
        public Task<WorkingHour?> GetWorkingHourDyId(int Id);
        public Task<IQueryable<GetAllWorkingHoureResponseDto>> GetAllWorkingHoure();
        public Task<bool> SaveChangesAsync();
        public Task CreateLocation(Location location);
        public void UpdateLocation(Location location);
        public Task<bool> GetLocationForAdd(AddLocationRequestDto addLocationRequestDto);
        public Task<bool> GetLocationByParForUpdate(UpdateLocationRequestDto updateLocationRequestDto);
        public Task<Location?> GetLocationDyId(int Id);
        public Task<IQueryable<GetAllLocationResponseDto>> GetAllLocations();

        public Task CreateWorkingDay(WorkingDay workingDay);
        public void UpdateWorkingDay(WorkingDay workingDay);
        public Task<bool> GetWorkingDayForAdd(AddWorkingDayRequestDto addWorkingDayRequestDto);
        public Task<bool> GetWorkingDayByParForUpdate(UpdateWorkingDayRequestDto updateWorkingDayRequestDto);
        public Task<WorkingDay?> GetWorkingDayDyId(int Id);
        public Task<IQueryable<GetAllWorkingDayResponseDto>> GetAllWorkingDays();


        public Task<Location?> GetUserLocation(int userId);
        public Task CreateAttendance(Attendenc attendenc);

        public Task<bool> IsExits(int userId, DateTime date);
        public Task<bool> CheckSerialNumber(int userId, string serialNumber);

        public Task<Attendenc> GetAttendance(int userId, DateTime date);
        public void UpdateAttendance(Attendenc attendenc);

        public Task<List<AttendanceRecordResponceDto>> GetAttendanceRecords(int userId, DateTime from, DateTime to);
    }

    public class AttendanceRepo : IAttendanceRepo
    {

        private readonly DataContext _context;

        public AttendanceRepo(DataContext context)
        {
            _context = context;
        }

        #region Working Hour
        public async Task CreateWorkingHour(WorkingHour workingHour)
        {
            await _context.WorkingHours.AddAsync(workingHour);
        }


        public void UpdateWorkingHour(WorkingHour workingHour)
        {
            _context.WorkingHours.Update(workingHour);
        }

        public async Task<bool> GetWorkingHourForAdd(AddWorkingHourRequestDto addWorkingHourDto)
        {
            return await _context.WorkingHours.AnyAsync(x => x.Start == addWorkingHourDto.Start || x.End == addWorkingHourDto.End);
        }

        public async Task<bool> SaveChangesAsync()
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

        public async Task<WorkingHour?> GetWorkingHourDyId(int Id)
        {
            var x = await _context.WorkingHours.FirstOrDefaultAsync(x => x.Id == Id);

            _context.ChangeTracker.Clear();
            return x;
        }

        public async Task<bool> GetWoorkingHourByParForUpdate(UpdateWorkingHourRequestDto updateWorkingHourDto)
        {
            return await _context.WorkingHours.AnyAsync(x => (x.Start == updateWorkingHourDto.Start || x.End == updateWorkingHourDto.End) && x.Id != updateWorkingHourDto.Id);
        }


        public async Task<IQueryable<GetAllWorkingHoureResponseDto>> GetAllWorkingHoure()
        {
            return await Task.FromResult(
                from w in _context.WorkingHours
                select new GetAllWorkingHoureResponseDto
                {
                    Id = w.Id,
                    Start = w.Start,
                    End = w.End
                }
            );
        }
        #endregion

        #region Location
        public async Task CreateLocation(Location location)
        {
            await _context.Locations.AddAsync(location);
        }

        public void UpdateLocation(Location location)
        {
            _context.Locations.Update(location);
        }

        public async Task<bool> GetLocationForAdd(AddLocationRequestDto addLocationRequestDto)
        {
            return await _context.Locations.AnyAsync(x => x.Longitude == addLocationRequestDto.Longitude || x.Latitude == addLocationRequestDto.Latitude);
        }

        public async Task<bool> GetLocationByParForUpdate(UpdateLocationRequestDto updateLocationRequestDto)
        {
            return await _context.Locations.AnyAsync(x => (x.Longitude == updateLocationRequestDto.Longitude || x.Latitude == updateLocationRequestDto.Latitude) && x.Id != updateLocationRequestDto.Id);
        }

        public async Task<Location?> GetLocationDyId(int Id)
        {
            var x = await _context.Locations.FirstOrDefaultAsync(x => x.Id == Id);

            _context.ChangeTracker.Clear();
            return x;
        }

        public async Task<IQueryable<GetAllLocationResponseDto>> GetAllLocations()
        {
            return await Task.FromResult(
               from l in _context.Locations
               select new GetAllLocationResponseDto
               {
                   Id = l.Id,
                   NameAr = l.NameAr,
                   NameEn = l.NameEn,
                   Longitude = l.Longitude,
                   Latitude = l.Latitude,
               }
           );
        }

        #endregion


        #region Woking Day

        public async Task CreateWorkingDay(WorkingDay workingDay)
        {
            await _context.WorkingDays.AddAsync(workingDay);
        }

        public void UpdateWorkingDay(WorkingDay workingDay)
        {
            _context.WorkingDays.Update(workingDay);
        }

        public async Task<bool> GetWorkingDayForAdd(AddWorkingDayRequestDto addWorkingDayRequestDto)
        {
            return await _context.WorkingDays.AnyAsync(x => x.NameAr == addWorkingDayRequestDto.NameAr || x.NameEn == addWorkingDayRequestDto.NameEn);
        }

        public async Task<bool> GetWorkingDayByParForUpdate(UpdateWorkingDayRequestDto updateWorkingDayRequestDto)
        {
            return await _context.WorkingDays.AnyAsync(x => (x.NameAr == updateWorkingDayRequestDto.NameAr || x.NameEn == updateWorkingDayRequestDto.NameEn) && x.Id != updateWorkingDayRequestDto.Id);
        }

        public async Task<WorkingDay?> GetWorkingDayDyId(int Id)
        {
            var x = await _context.WorkingDays.FirstOrDefaultAsync(x => x.Id == Id);

            _context.ChangeTracker.Clear();
            return x;
        }

        public async Task<IQueryable<GetAllWorkingDayResponseDto>> GetAllWorkingDays()
        {
            return await Task.FromResult(
               from wd in _context.WorkingDays
               select new GetAllWorkingDayResponseDto
               {
                   Id = wd.Id,
                   NameAr = wd.NameAr,
                   NameEn = wd.NameEn,
               }
           );
        }



        #endregion


        #region Attendance


        public async Task<Location?> GetUserLocation(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user != null)
            {
                var location = await _context.Locations.FindAsync(user.LocationId);
                return location;
            }

            return null;
        }

        public async Task CreateAttendance(Attendenc attendenc)
        {
            await _context.Attendencs.AddAsync(attendenc);
        }
        public void UpdateAttendance(Attendenc attendenc)
        {
            _context.Attendencs.Update(attendenc);
        }

        public async Task<bool> IsExits(int userId, DateTime date)
        {
            return await _context.Attendencs.AnyAsync(x => x.UserId == userId && x.Date.Date == date.Date);
        }


        public async Task<bool> CheckSerialNumber(int userId, string serialNumber)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user?.SerialNumber == serialNumber;
        }

        public async Task<Attendenc> GetAttendance(int userId, DateTime date)
        {
            var attendance = await _context.Attendencs.FirstOrDefaultAsync(x => x.UserId == userId && x.Date.Date == date.Date);
            if (attendance == null)
            {
                throw new Exception("Attendance not found.");
            }
            return attendance;
        }




        public async Task<List<AttendanceRecordResponceDto>> GetAttendanceRecords(int userId, DateTime from, DateTime to)
        {
            var workingDayIds = await _context.User_WorkingDays
                .Where(x => x.UserId == userId)
                .Select(x => x.WorkingDayId)
                .ToListAsync();

            var workingDaysNames = new HashSet<string>(await _context.WorkingDays
                .Where(wd => workingDayIds.Contains(wd.Id))
                .Select(wd => wd.NameEn)
                .ToListAsync());

            var WorkingHourId = await _context.Users
            .Where(x => x.Id == userId)
            .Select(x => x.WorkingHourId)
            .FirstOrDefaultAsync();

            var workingHours = await _context.WorkingHours
                .Where(x => x.Id == WorkingHourId)
                .Select(x => x.Start)
                .FirstOrDefaultAsync();


            var matchingDates = new List<DateTime>();

            for (DateTime date = from.Date; date.Date <= to.Date; date = date.AddDays(1))
            {
                if (workingDaysNames.Contains(date.DayOfWeek.ToString()))
                {
                    matchingDates.Add(date.Date);
                }
            }

            var attendanceRecords = new Dictionary<DateTime, AttendanceRecordResponceDto>();

            var attendances = await _context.Attendencs
    .Where(x => x.UserId == userId && x.Date.Date >= from.Date && x.Date.Date <= to.Date)
    .ToListAsync();

            var timeOffs = await _context.TimeOffs
                .Where(x => x.UserId == userId && x.CreatedAt.Date >= from.Date && x.CreatedAt.Date <= to.Date)
                .ToListAsync();

            var holidays = await _context.PublicLists
     .Where(x => x.Type == 1092)
     .ToListAsync();

            var filteredHolidays = holidays
                .Where(x => Convert.ToDateTime(x.NameEN).Date >= from.Date && Convert.ToDateTime(x.NameEN).Date <= to.Date)
                .ToList();


            foreach (var date in matchingDates)
            {
                var attendanceRecord = new AttendanceRecordResponceDto
                {
                    Date = date.Date,
                    Status = "Absent", // Default status is Absent
                    CheckInTime = null,
                    CheckOutTime = null,
                    Daley=null,
                };

                var attendance = attendances.FirstOrDefault(x => x.Date.Date == date.Date);
                if (attendance != null)
                {
                    attendanceRecord.Status = "Present";
                    attendanceRecord.CheckInTime = attendance.CheckIn;
                    attendanceRecord.CheckOutTime = attendance.CheckOut;
                    if (workingHours != null)
                    {
                        TimeSpan s = TimeSpan.Parse(workingHours);
                        TimeSpan delay = attendance.CheckIn - s;
                        if (delay < TimeSpan.Zero)
                        {
                            delay = TimeSpan.Zero;
                        }

                        attendanceRecord.Daley = delay;
                    }
                    
                }

                var timeOff = timeOffs.FirstOrDefault(x => x.CreatedAt.Date == date.Date);
                if (timeOff != null)
                {
                    attendanceRecord.Status = "Time Off";
                }

                var holiday = holidays.FirstOrDefault(x => Convert.ToDateTime(x.NameEN).Date >= from.Date && Convert.ToDateTime(x.NameEN).Date <= to.Date);
                if (holiday != null)
                {
                    attendanceRecord.Status = "Holiday";
                }

                attendanceRecords.Add(date, attendanceRecord);
            }

            return attendanceRecords.Values.ToList();
        }


        #endregion
    }
}
