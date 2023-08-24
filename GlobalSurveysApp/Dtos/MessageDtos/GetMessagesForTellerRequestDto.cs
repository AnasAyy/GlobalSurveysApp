using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.MessageDtos
{
    public class GetMessagesForTellerRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Page { get; set; }
    }
}
