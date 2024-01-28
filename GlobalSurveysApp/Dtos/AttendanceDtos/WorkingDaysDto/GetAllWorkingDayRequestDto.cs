using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.AttendanceDtos.WorkingHourDto
{
    public class GetAllWorkingDayRequestDto
    {

        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Page { get; set; }


    }
}
