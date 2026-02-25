using BookWorm.Model.Common;

namespace BookWorm.Model
{
    public class BookTitle : EntityBase, IBookTitle
    {
        public string Title { get; set; } = null!;
        public string? Subtitle { get; set; }= null;
        public string AuthorName { get; set; } = null!; 
        public string LanguageName { get; set; } = null!; 
        public string TagLineText { get; set; } = null!;
        public TagLine TagLine { get; set; } = null!;

        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    }
}
