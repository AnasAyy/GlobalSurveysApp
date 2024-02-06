using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.AttendanceDtos.AttendanceDto
{
    public class AttendanceReportForManagementDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
    }
}
