using AutoMapper;
using GlobalSurveysApp.Dtos.TimeOffDtos;
using GlobalSurveysApp.Models;

namespace GlobalSurveysApp.Profiles
{
    public class TimeOffProfile : Profile
    {
        public TimeOffProfile() 
        {
            CreateMap<TimeOff, AddTimeOffRequestDto>().ReverseMap();
        }
    }
}
