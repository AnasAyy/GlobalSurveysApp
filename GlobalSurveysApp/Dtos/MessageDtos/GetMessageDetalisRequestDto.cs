using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.MessageDtos
{
    public class GetMessageDetalisRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Id { get; set; }
    }
}
