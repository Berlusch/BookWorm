using Bookworm.Repository.Common;
using BookWorm.Common;
using BookWorm.Model;
using BookWorm.Service.Common;
using Microsoft.EntityFrameworkCore;
namespace BookWorm.Service
{
    public class BookTitleService : IBookTitleService
    {
        private readonly IBookTitleRepository _bookTitleRepository;
        private readonly IGenreRepository _genreRepository;
        public BookTitleService(IBookTitleRepository bookTitleRepository, IGenreRepository genreRepository)
        {
            _bookTitleRepository = bookTitleRepository;
            _genreRepository = genreRepository;
        }
        public async Task<PagedResult<BookTitle>> GetBookTitlesAsync(PagingParameters paging, SortingParameters sorting, FilterParameters filter)
        {
            var query = _bookTitleRepository.GetQuery(paging, sorting, filter)
                .Include(b => b.Author)
                .Include(b => b.Language)
                .Include(b => b.TagLine)
                .Include(b => b.Genres);
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip(paging.Skip)
                .Take(paging.PageSize)
                .ToListAsync();
            return new PagedResult<BookTitle>
            {
                Items = items.ToList(),
                TotalCount = totalCount,
                Paging = paging
            };
        }
        public async Task<BookTitle?> GetByIdWithDetailsAsync(int id)
        {
            return await _bookTitleRepository.GetByIdWithDetailsAsync(id);
        }
        public async Task<BookTitle> AddBookTitleAsync(BookTitle bookTitle, List<int> genreIds)
        {
            var genres = await _genreRepository.GetQuery(new PagingParameters(), new SortingParameters(), new FilterParameters())
                .Where(g => genreIds.Contains(g.Id))
                .ToListAsync();
            bookTitle.Genres = genres;
            var added = await _bookTitleRepository.AddAsync(bookTitle);
            return await _bookTitleRepository.GetByIdWithDetailsAsync(added.Id)
                ?? throw new KeyNotFoundException($"BookTitle with ID {added.Id} not found.");
        }
        public async Task<BookTitle> UpdateBookTitleAsync(int id, BookTitle bookTitle, List<int> genreIds)
        {
            var existing = await _bookTitleRepository.GetByIdWithDetailsAsync(id)
                ?? throw new KeyNotFoundException($"BookTitle with ID {id} not found.");
            existing.Title = bookTitle.Title;
            existing.Subtitle = bookTitle.Subtitle;
            existing.AuthorId = bookTitle.AuthorId;
            existing.LanguageId = bookTitle.LanguageId;
            var genres = await _genreRepository.GetQuery(new PagingParameters(), new SortingParameters(), new FilterParameters())
                .Where(g => genreIds.Contains(g.Id))
                .ToListAsync();
            existing.Genres = genres;
            await _bookTitleRepository.UpdateAsync(existing);
            return await _bookTitleRepository.GetByIdWithDetailsAsync(existing.Id)
                ?? throw new KeyNotFoundException($"BookTitle with ID {existing.Id} not found.");
        }
        public async Task<bool> DeleteBookTitleAsync(int id)
        {
            return await _bookTitleRepository.DeleteAsync(id);
        }
    }
}