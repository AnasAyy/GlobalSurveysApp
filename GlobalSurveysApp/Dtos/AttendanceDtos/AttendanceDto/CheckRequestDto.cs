using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.AttendanceDtos.AttendanceDto
{
    public class CheckRequestDto
    {
        
        [Required]
        public double DesignatedLat {  get; set; }
        [Required]
        public double DesignatedLon { get; set; }

        [Required]
        public string SerialNumber { get; set; } = null!;
    }
}
