using GlobalSurveysApp.Dtos.AdvanceDtos;
using GlobalSurveysApp.Models;
using AutoMapper;
using GlobalSurveysApp.Dtos.AttendanceDtos.WorkingHourDto;
using GlobalSurveysApp.Dtos.AttendanceDtos.LocationDto;

namespace GlobalSurveysApp.Profiles
{
    public class AttendanceProfile : Profile
    {
        public AttendanceProfile()
        {
            CreateMap<WorkingHour, AddWorkingHourRequestDto>().ReverseMap();
            CreateMap<WorkingHour, UpdateWorkingHourRequestDto>().ReverseMap();
            
            
            CreateMap<Location, AddLocationRequestDto>().ReverseMap();
            CreateMap<Location, UpdateLocationRequestDto>().ReverseMap();

            CreateMap<WorkingDay, AddWorkingDayRequestDto>().ReverseMap();
            CreateMap<WorkingDay, UpdateWorkingDayRequestDto>().ReverseMap();

        }

        
    }
}
