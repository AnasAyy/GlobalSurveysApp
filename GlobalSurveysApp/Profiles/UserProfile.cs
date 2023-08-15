using AutoMapper;
using GlobalSurveysApp.Dtos.UserManagmentDtos.LoginManagement;
using GlobalSurveysApp.Dtos.UserManagmentDtos.UserDtos;
using GlobalSurveysApp.Dtos.UserManagmentDtos.UserRequest;
using GlobalSurveysApp.Models;

namespace GlobalSurveysApp.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<User, UserResponceDto>();
            CreateMap<User, AddUserRequestDto>().ReverseMap().ForMember(dest => dest.IdCard, opt => opt.Ignore());
            CreateMap<User, UpdateUserRequestDto>().ReverseMap();
            CreateMap<User, ViewUserResponceDto>().ReverseMap();
        }
    }
}
