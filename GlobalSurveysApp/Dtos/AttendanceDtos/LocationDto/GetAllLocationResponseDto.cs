namespace GlobalSurveysApp.Dtos.AttendanceDtos.LocationDto
{
    public class GetAllLocationResponseDto
    {

        public int Id { get; set; }

        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;

        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }
}
