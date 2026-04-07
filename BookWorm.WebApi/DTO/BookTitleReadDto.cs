public class BookTitleReadDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Subtitle { get; set; }
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = null!;
    public int LanguageId { get; set; }
    public string LanguageName { get; set; } = null!;
    public List<string> BookQuotes { get; set; } = new();
    public List<string> Genres { get; set; } = new();
    public List<int> GenreIds { get; set; } = new();
}