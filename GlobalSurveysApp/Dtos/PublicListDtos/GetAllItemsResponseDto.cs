using GlobalSurveysApp.Models;
using System.Security;

namespace GlobalSurveysApp.Dtos.PublicListDtos
{
    public class GetAllItemsResponseDto
    {
        public int Id { get; set; }
        public string NameAR { get; set; } = null!;
        public string NameEN { get; set; } = null!;
        

    }

   
}
