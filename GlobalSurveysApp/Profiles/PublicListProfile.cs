using AutoMapper;
using GlobalSurveysApp.Dtos.PublicListDtos;
using GlobalSurveysApp.Models;

namespace GlobalSurveysApp.Profiles
{
    public class PublicListProfile : Profile
    {
        public PublicListProfile() 
        {
            CreateMap<PublicList, AddItemRequestDto>().ReverseMap();
            CreateMap<PublicList, UpdateItemRequestDto>().ReverseMap();
            CreateMap<PublicList, GetAllMainItemsResponseDto>();
           
        }
    }
}
