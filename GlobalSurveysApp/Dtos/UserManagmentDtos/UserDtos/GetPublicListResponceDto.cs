namespace GlobalSurveysApp.Dtos.UserManagmentDtos.UserDtos
{
    public class GetPublicListResponceDto
    {
        public List<Publics> PlaceOfBirth { get; set; } = null!;
        public List<Publics> CertificateLevel { get; set; } = null!;
        public List<Publics> FieldOfStudy { get; set; } = null!;
        public List<Publics> Gender { get; set; } = null!;
        public List<Publics> Postion { get; set; } = null!;
        public List<Publics> Nationality { get; set; } = null!;
        public List<Publics> Department { get; set; } = null!;
        public List<Publics> Location { get; set; } = null!;

    }
    public class Publics
    {
        public int Id { get; set; }
        public string NameAR { get; set; } = null!;
        public string NameEN { get; set; } = null!;
    }
}
