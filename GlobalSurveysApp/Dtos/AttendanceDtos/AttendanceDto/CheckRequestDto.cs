using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.AttendanceDtos.AttendanceDto
{
    public class CheckRequestDto
    {
        [Required]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):([0-5][0-9])$", ErrorMessage = "Invalid input")]
        public string Time { get; set; } = null!;
        [Required]
        public double DesignatedLat {  get; set; }
        [Required]
        public double DesignatedLon { get; set; }

        [Required]
        public string SerialNumber { get; set; } = null!;
    }
}
