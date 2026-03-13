using Bookworm.Repository.Common;
using BookWorm.Common;
using BookWorm.Model;
using BookWorm.Service.Common;
using Microsoft.EntityFrameworkCore;

namespace BookWorm.Service
{
    public class BookQuoteService : IBookQuoteService
    {
        private readonly IBookQuoteRepository _bookQuoteRepository;

        public BookQuoteService(IBookQuoteRepository bookQuoteRepository)
        {
            _bookQuoteRepository = bookQuoteRepository;
        }

        public async Task<PagedResult<BookQuote>> GetBookQuotesAsync(PagingParameters paging, SortingParameters sorting, FilterParameters filter)
        {
            var query = _bookQuoteRepository.GetQuery(paging, sorting, filter)
                .Include(q => q.BookTitle);
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip(paging.Skip)
                .Take(paging.PageSize)
                .ToListAsync();

            return new PagedResult<BookQuote>
            {
                Items = items.ToList(),
                TotalCount = totalCount,
                Paging = paging
            };
        }

        public async Task<BookQuote> GetBookQuoteByIdAsync(int id)
        {
            return await _bookQuoteRepository.GetQuery(new PagingParameters(), new SortingParameters(), new FilterParameters())
                .Include(q => q.BookTitle)
                .FirstOrDefaultAsync(q => q.Id == id)
                ?? throw new KeyNotFoundException($"BookQuote with ID {id} not found.");
        }

        public async Task<BookQuote> AddBookQuoteAsync(BookQuote bookQuote)
        {
            var added = await _bookQuoteRepository.AddAsync(bookQuote);
            return await _bookQuoteRepository.GetQuery(new PagingParameters(), new SortingParameters(), new FilterParameters())
                .Include(q => q.BookTitle)
                .FirstOrDefaultAsync(q => q.Id == added.Id)
                ?? throw new KeyNotFoundException($"BookQuote with ID {added.Id} not found.");
        }

        public async Task<BookQuote> UpdateBookQuoteAsync(int id, BookQuote bookQuote)
        {
            var existing = await _bookQuoteRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"BookQuote with ID {id} not found.");

            existing.Text = bookQuote.Text;
            existing.BookTitleId = bookQuote.BookTitleId;

            await _bookQuoteRepository.UpdateAsync(existing);
            return await _bookQuoteRepository.GetQuery(new PagingParameters(), new SortingParameters(), new FilterParameters())
                .Include(q => q.BookTitle)
                .FirstOrDefaultAsync(q => q.Id == id)
                ?? throw new KeyNotFoundException($"BookQuote with ID {id} not found.");
        }

        public async Task<bool> DeleteBookQuoteAsync(int id)
        {
            return await _bookQuoteRepository.DeleteAsync(id);
        }
    }
}