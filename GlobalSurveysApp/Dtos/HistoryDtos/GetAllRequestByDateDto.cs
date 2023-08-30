using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.HistoryDtos
{
    public class GetAllRequestByDateDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int UserId { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Page { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
