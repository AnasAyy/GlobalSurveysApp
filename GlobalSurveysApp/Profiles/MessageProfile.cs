using AutoMapper;
using GlobalSurveysApp.Dtos.AdvanceDtos;
using GlobalSurveysApp.Dtos.MessageDtos;
using GlobalSurveysApp.Models;

namespace GlobalSurveysApp.Profiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile() 
        {
            CreateMap<Message, AddMessageRequestDto>().ReverseMap();
        }
    }
}
