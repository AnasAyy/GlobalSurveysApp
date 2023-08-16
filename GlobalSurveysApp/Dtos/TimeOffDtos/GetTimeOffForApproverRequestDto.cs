using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.TimeOffDtos
{
    public class GetTimeOffForApproverRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Page { get; set; }
    }
}
