namespace GlobalSurveysApp.Dtos.AdvanceDtos
{
    public class ApproveRequestDto
    {
        public int RequestId { get; set; }
        public int Status { get; set; }
        public string Note { get; set; } = null!;
    }
}
