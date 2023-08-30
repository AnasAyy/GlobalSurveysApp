using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.HistoryDtos
{
    public class GetTotalByDateRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int UserId { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
