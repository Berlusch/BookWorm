using BookWorm.Model.Common;

namespace BookWorm.Model
{
    public class Author : EntityBase, IAuthor
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName => $"{FirstName} {LastName}";
        public int? BirthYear { get; set; }
        public int? DeathYear { get; set; }
        public string Biography { get; set; } = null!;
        public string NationalLiterature { get; set; } = null!;

        // Navigation 1:N to BookTitle
        public ICollection<BookTitle> BookTitles { get; set; } = new List<BookTitle>();
    }
}
