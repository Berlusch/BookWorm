using BookWorm.Common;
using BookWorm.Model;

namespace BookWorm.Service.Common
{
    public interface IBookQuoteService
    {
        Task<PagedResult<BookQuote>> GetBookQuotesAsync(PagingParameters paging, SortingParameters sorting, FilterParameters filter);
        Task<BookQuote> GetBookQuoteByIdAsync(int id);
        Task<BookQuote> AddBookQuoteAsync(BookQuote bookQuote);
        Task<BookQuote> UpdateBookQuoteAsync(int id, BookQuote bookQuote);
        Task<bool> DeleteBookQuoteAsync(int id);
    }
}