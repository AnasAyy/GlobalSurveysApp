using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.HistoryDtos
{
    public class GetListOfUsersByNameRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Page { get; set; }

        [RegularExpression(@"^[\p{L}\p{M}' \.\-]+$", ErrorMessage = "Invalid Input")]
        public string Name { get; set; } = null!;
    }
}
