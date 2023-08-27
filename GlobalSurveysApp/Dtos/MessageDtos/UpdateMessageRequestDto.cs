using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.MessageDtos
{
    public class UpdateMessageRequestDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Id { get; set; }
        [RegularExpression(@"^[\p{L}\p{M}' \.\-]+$", ErrorMessage = "Invalid Input")]
        public string Title { get; set; } = null!;
        [RegularExpression(@"^[\p{L}\p{M}' \.\-]+$", ErrorMessage = "Invalid Input")]
        public string Body { get; set; } = null!;
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]

        public int Type { get; set; }
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]

        public int ToWhom { get; set; }

    }
}
