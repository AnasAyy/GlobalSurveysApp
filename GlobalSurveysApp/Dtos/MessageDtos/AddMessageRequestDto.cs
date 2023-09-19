using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.MessageDtos
{
    public class AddMessageRequestDto
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Body is required")]
        public string Body { get; set; } = null!;
        
        
        [Required(ErrorMessage = "Type is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Type { get; set; }

        [Required(ErrorMessage = "ToWhom is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int ToWhom { get; set; }
        
    }
}
