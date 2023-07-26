using AutoMapper;
using GlobalSurveysApp.Dtos.UserManagmentDtos;
using GlobalSurveysApp.Models;

namespace GlobalSurveysApp.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<User, UserResponceDto>();
        }
    }
}
