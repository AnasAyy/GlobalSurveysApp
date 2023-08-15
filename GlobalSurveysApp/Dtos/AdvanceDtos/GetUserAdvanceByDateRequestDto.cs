namespace GlobalSurveysApp.Dtos.AdvanceDtos
{
    public class GetUserAdvanceByDateRequestDto
    {
        public int Page { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
