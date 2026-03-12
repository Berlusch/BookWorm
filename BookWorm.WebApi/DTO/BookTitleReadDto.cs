namespace BookWorm.WebApi.DTO
{
    public class BookTitleReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Subtitle { get; set; }
        public string AuthorName { get; set; } = null!;
        public string LanguageName { get; set; } = null!;
        public string TagLineText { get; set; } = null!;
        public List<string> Genres { get; set; } = new();
    }
}