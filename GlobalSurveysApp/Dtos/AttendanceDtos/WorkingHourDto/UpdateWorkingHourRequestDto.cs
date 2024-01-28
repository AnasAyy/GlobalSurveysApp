using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.AttendanceDtos.WorkingHourDto
{
    public class UpdateWorkingHourRequestDto
    {
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):([0-5][0-9])$", ErrorMessage = "Invalid input")]
        public string Start { get; set; } = null!;

        [Required]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):([0-5][0-9])$", ErrorMessage = "Invalid input")]
        public string End { get; set; } = null!;





    }
}
