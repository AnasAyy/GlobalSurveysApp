namespace GlobalSurveysApp.Dtos.PublicListDtos
{
    public class UpdateItemRequestDto
    {
        public int Id { get; set; }
        public string NameAR { get; set; } = null!;
        public string NameEN { get; set; } = null!;
        public int Type { get; set; }
    }
}
