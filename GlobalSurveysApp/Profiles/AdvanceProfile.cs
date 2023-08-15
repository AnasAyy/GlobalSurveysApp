using AutoMapper;
using GlobalSurveysApp.Dtos.AdvanceDtos;
using GlobalSurveysApp.Dtos.PublicListDtos;
using GlobalSurveysApp.Models;

namespace GlobalSurveysApp.Profiles
{
    public class AdvanceProfile : Profile
    {
        public AdvanceProfile()
        {
            CreateMap<Advance, AddAdvanceRequestDto>().ReverseMap();
            CreateMap<Advance, UpdateAdvanceRequestDto>().ReverseMap();

        }
    }

}
