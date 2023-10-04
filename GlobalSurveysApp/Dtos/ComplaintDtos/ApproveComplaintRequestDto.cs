using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.ComplaintDtos
{
    public class ApproveComplaintRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int RequestId { get; set; }
        [Required]
        public string Note { get; set; } = null!;
    }
}
