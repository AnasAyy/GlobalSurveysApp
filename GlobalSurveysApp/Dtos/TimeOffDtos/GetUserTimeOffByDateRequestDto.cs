using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.TimeOffDtos
{
    public class GetUserTimeOffByDateRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Page { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
