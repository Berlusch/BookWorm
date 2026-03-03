namespace BookWorm.WebApi.DTO
{
    public class AuthorInsertUpdateDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int? BirthYear { get; set; }
        public int? DeathYear { get; set; }
        public string Biography { get; set; } = null!;
        public string NationalLiterature { get; set; } = null!;
    }
}
