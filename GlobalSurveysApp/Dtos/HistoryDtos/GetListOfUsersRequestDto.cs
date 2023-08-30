using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.HistoryDtos
{
    public class GetListOfUsersRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Page { get; set; }
    }
}
