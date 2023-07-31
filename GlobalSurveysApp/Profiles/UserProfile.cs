using AutoMapper;
using GlobalSurveysApp.Dtos.UserManagmentDtos.LoginManagement;
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
