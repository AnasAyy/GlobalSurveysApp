namespace GlobalSurveysApp.Dtos.UserManagmentDtos.UserDtos
{
    public class GetUserByNameRequestDto
    {
        public int Page { get; set; }
        public string Name { get; set; } = null!;
    }
}
