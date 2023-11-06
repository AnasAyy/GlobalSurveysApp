namespace GlobalSurveysApp.Dtos.TimeOffDtos
{
    public class TimeOffsForSubEmpResponceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime From { get; set; }
        public int Type {  get; set; }
        public string Number { get; set; } = null!;
    }
}
