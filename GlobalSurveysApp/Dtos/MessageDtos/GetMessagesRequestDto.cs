using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.MessageDtos
{
    public class GetMessagesRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Page { get; set; }
    }
}
