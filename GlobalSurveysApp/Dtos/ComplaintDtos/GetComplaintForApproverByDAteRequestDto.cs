using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.ComplaintDtos
{
    public class GetComplaintForApproverByDAteRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Page { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
