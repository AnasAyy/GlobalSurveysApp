using GlobalSurveysApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.AdvanceDtos
{
    public class GetAllAdvanceResponseDto
    {
       
        public int Id { get; set; }
        
        public int Amount { get; set; }
        
        public bool IsUpdated { get; set; }
        public DateTime CreateAt { get; set; }
        
        public RequestStatus Status { get; set; } 
        
    }
}
