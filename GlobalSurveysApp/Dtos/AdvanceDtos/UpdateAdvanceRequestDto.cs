using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.AdvanceDtos
{
    public class UpdateAdvanceRequestDto
    {

        
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Id { get; set; }
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Amount { get; set; }
        
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Currency { get; set; }
        
        public DateTime From { get; set; }
        
        public DateTime To { get; set; }
    }
}
