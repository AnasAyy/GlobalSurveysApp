using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.HistoryDtos
{
    public class GetAllRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int UserId { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Page { get; set; }
    }
}
