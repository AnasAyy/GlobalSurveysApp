using GlobalSurveysApp.Models;

namespace GlobalSurveysApp.Dtos.ComplaintDtos
{
    public class GetAllComplaintResponseDto
    {
        public int Id { get; set; }
        public int Title { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsUpdated { get; set; }
        public RequestStatus Status { get; set; }

    }
}
