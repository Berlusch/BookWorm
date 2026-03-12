namespace BookWorm.WebApi.DTO
{
    public class BookTitleInsertUpdateDto
    {
        public string Title { get; set; } = null!;
        public string? Subtitle { get; set; }
        public int AuthorId { get; set; }
        public int LanguageId { get; set; }
        public List<int> GenreIds { get; set; } = new();
    }
}