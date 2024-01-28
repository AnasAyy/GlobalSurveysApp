using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.AttendanceDtos.WorkingHourDto
{
    public class AddWorkingHourRequestDto
    {
        [Required]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):([0-5][0-9])$", ErrorMessage = "Invalid input")]
        public string Start { get; set; } = null!;

        [Required]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):([0-5][0-9])$", ErrorMessage = "Invalid input")]
        public string End { get; set; } = null!;

    }
}
