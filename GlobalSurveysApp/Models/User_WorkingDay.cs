using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Models
{
    public class User_WorkingDay
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        
        public int WorkingDayId { get; set; }
        
        
    }
}
