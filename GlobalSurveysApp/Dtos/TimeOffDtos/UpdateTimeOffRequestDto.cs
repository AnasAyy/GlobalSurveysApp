using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.TimeOffDtos
{
    public class UpdateTimeOffRequestDto
    {
        [Required(ErrorMessage = "Id is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Id { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Type { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string Number { get; set; } = null!; 

        public string EmergencyNumber { get; set; } = null!;

        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int SubstituteEmployeeId { get; set; }
    }
}
