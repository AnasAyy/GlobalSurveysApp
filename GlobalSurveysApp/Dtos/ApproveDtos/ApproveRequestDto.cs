using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.ApproveDtos
{
    public class ApproveRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int RequestId { get; set; }
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Status { get; set; }
        public string Note { get; set; } = null!;
    }
}
