using BookWorm.Model;

public class BookTitle : EntityBase, IBookTitle
{
    public string Title { get; set; } = null!;
    public string? Subtitle { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    public int LanguageId { get; set; }
    public Language Language { get; set; } = null!;

    public TagLine TagLine { get; set; } = null!;
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
        
    string IBookTitle.AuthorName => Author.FullName;
    string IBookTitle.LanguageName => Language.Name;
    string IBookTitle.TagLineText => TagLine.Text;
}