using GlobalSurveysApp.Models;

namespace GlobalSurveysApp.Dtos.TimeOffDtos
{
    public class GetTimeOffDetailsResponseDto
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Number { get; set; } = null!;
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public string EmergencyNumber { get; set; } = null!;
        public int SubsituteEmployeeId { get; set; }
        public RequestStatus? SubEmpStatus { get; set; } = null;
        public RequestStatus? DirectResponsibleStatus { get; set; } = null;
        public RequestStatus? HRStatus { get; set; } = null;
        public RequestStatus? ManagerStatus { get; set; } = null;
        public string StatusNote { get; set; } = null!;
    }
}
