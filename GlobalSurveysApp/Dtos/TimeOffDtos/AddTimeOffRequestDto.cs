using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.TimeOffDtos
{
    public class AddTimeOffRequestDto
    {
        [Required(ErrorMessage = "Type is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Type { get; set; }

        [Required(ErrorMessage = "From Date is required")]
        public DateTime From { get; set; }

        [Required(ErrorMessage = "From Date is required")]
        public DateTime To { get; set; }

        [Required(ErrorMessage = "Number is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Number { get; set; }

        [Required(ErrorMessage = "Emergency Number is required")]
        public string EmergencyNumber { get; set; } = null!;

        [Required(ErrorMessage = "Substitute Employee is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int SubstituteEmployeeId { get; set; }

    }
}
