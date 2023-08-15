using GlobalSurveysApp.Models;

namespace GlobalSurveysApp.Dtos.AdvanceDtos
{
    public class GetAdvanceDetailsResponseDto
    {
       public int Id { get; set; }
       public int Amount { get; set;}
       public int Currency { get; set;}
       public DateTime From { get; set;} 
       public DateTime To { get; set;}
        public RequestStatus? DirectResponsibleStatus { get; set; } = null;
        public RequestStatus? HRStatus { get; set; } = null;
       public RequestStatus? ManagerStatus { get; set; } = null;
       public string StatusNote { get; set; } = null!;
        
    }
}
