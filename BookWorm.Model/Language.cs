using BookWorm.Model.Common;

namespace BookWorm.Model
{
    public class Language : EntityBase, ILanguage
    {
        public string Name { get; set; } = null!;

        // Navigation 1:N
        public ICollection<BookTitle> BookTitles { get; set; } = new List<BookTitle>();
    }
}
