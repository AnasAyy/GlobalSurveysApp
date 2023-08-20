using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.ComplaintDtos
{
    public class GetComplaintForApproverRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Page { get; set; }
    }
}
