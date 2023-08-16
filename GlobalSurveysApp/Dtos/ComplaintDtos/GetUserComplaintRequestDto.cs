using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.ComplaintDtos
{
    public class GetUserComplaintRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Page { get; set; }
    }
}
