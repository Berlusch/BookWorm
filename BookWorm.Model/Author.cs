using BookWorm.Model.Common;

namespace BookWorm.Model
{
    public class Author : EntityBase, IAuthor
    {
        public string FullName { get; set; } = null!;
        public int? BirthYear { get; set; }
        public int? DeathYear { get; set; }
        public string Biography { get; set; } = null!;
        public string NationalLiterature { get; set; } = null!;

        // Navigation 1:N to BookTitle
        public ICollection<BookTitle> BookTitles { get; set; } = new List<BookTitle>();
    }
}
