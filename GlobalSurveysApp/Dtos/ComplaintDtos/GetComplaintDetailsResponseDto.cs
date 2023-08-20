using GlobalSurveysApp.Models;

namespace GlobalSurveysApp.Dtos.ComplaintDtos
{
    public class GetComplaintDetailsResponseDto
    {
        public int Id { get; set; }
        public int Title { get; set; }
        public string Details { get; set; } = null!;
        public string Against { get; set; } = null!;
        public RequestStatus Status { get; set; } 
        public string Note { get; set; } = null!;

    }
}
