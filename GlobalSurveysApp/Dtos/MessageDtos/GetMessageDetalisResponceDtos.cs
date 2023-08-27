namespace GlobalSurveysApp.Dtos.MessageDtos
{
    public class GetMessageDetalisResponceDtos
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public DateTime Date { get; set; } 
        public int Type { get; set; }
        public string ToWhomAR { get; set; } = null!;
        public string ToWhomEN { get; set; } = null!;
    }
}
