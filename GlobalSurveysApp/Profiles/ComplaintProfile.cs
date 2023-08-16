using AutoMapper;
using GlobalSurveysApp.Dtos.ComplaintDtos;
using GlobalSurveysApp.Dtos.PublicListDtos;
using GlobalSurveysApp.Models;

namespace GlobalSurveysApp.Profiles
{
    public class ComplaintProfile : Profile
    {
        public ComplaintProfile()
        {
            CreateMap<Complaint, AddComplaintRequestDto>().ReverseMap();
        }
    }
}
