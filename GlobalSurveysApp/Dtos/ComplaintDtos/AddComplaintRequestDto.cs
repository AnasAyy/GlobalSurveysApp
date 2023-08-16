using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.ComplaintDtos
{
    public class AddComplaintRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Title { get; set; }

        [RegularExpression(@"^[\p{L}\p{M}' \.\-]+$", ErrorMessage = "Invalid Input")]
        public string Description { get; set; } = null!;

        [RegularExpression(@"^[\p{L}\p{M}' \.\-]+$", ErrorMessage = "Invalid Input")]
        public string Against { get; set; } = null!;
    }
}
