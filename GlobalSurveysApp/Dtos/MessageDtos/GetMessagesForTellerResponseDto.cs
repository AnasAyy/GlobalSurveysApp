namespace GlobalSurveysApp.Dtos.MessageDtos
{
    public class GetMessagesForTellerResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }
        public string TypeAR { get; set; } = null!;
        public string TypeEN { get; set; } = null!;
    }
}
