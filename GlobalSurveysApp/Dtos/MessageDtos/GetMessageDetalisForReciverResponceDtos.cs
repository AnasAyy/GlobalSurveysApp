namespace GlobalSurveysApp.Dtos.MessageDtos
{
    public class GetMessageDetalisForReciverResponceDtos
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public DateTime Date { get; set; }
        public string From { get; set; } = null!;
        public int Type { get; set; }
        public string ToWhomAR { get; set; } = null!;
        public string ToWhomEN { get; set; } = null!;
    }
}
