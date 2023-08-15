using GlobalSurveysApp.Models;

namespace GlobalSurveysApp.Dtos.TimeOffDtos
{
    public class GetAllTimeOffResponseDto
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public bool IsUpdated { get; set; }
        public DateTime CreateAt { get; set; }
        public RequestStatus Status { get; set; }

    }
}
