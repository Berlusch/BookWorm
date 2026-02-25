using BookWorm.Model.Common;

namespace BookWorm.Model
{
    public class Genre : EntityBase, IGenre
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        // EF Core navigation M:N with BookTitle
        public ICollection<BookTitle> BookTitles { get; set; } = new List<BookTitle>();
    }
}
