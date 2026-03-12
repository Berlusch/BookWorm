using BookWorm.Common;
using BookWorm.Model;
namespace BookWorm.Service.Common
{
    public interface IBookTitleService
    {
        Task<PagedResult<BookTitle>> GetBookTitlesAsync(PagingParameters paging, SortingParameters sorting, FilterParameters filter);
        Task<BookTitle?> GetByIdWithDetailsAsync(int id);
        Task<BookTitle> AddBookTitleAsync(BookTitle bookTitle, List<int> genreIds);
        Task<BookTitle> UpdateBookTitleAsync(int id, BookTitle bookTitle, List<int> genreIds);
        Task<bool> DeleteBookTitleAsync(int id);
    }
}