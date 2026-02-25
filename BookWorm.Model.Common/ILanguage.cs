namespace BookWorm.Model.Common
{
    public interface ILanguage:IEntityBase
    {
        string Name { get; set; }

        // Navigation: BookTitles (1:N)
        ICollection<BookTitle> BookTitles { get; set; }
    }
}
