using System.ComponentModel.DataAnnotations;

namespace GlobalSurveysApp.Dtos.PublicListDtos
{
    public class AddItemRequestDto
    {
        public string NameAR { get; set; } = null!;
        public string NameEN { get; set; } = null!;
        public int Type { get; set; }
    }
}
