using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.AdvanceDtos
{
    public class AddAdvanceRequestDto
    {
        
        [Required(ErrorMessage = "Amount is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Currency is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid input")]
        public int Currency { get; set; }
        [Required(ErrorMessage = "From Date is required")]
        public DateTime From { get; set; }
        [Required(ErrorMessage = "To Date is required")]
        public DateTime To { get; set; }
    }
}
