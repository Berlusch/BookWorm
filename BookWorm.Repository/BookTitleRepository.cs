using Bookworm.Repository.Common;
using BookWorm.DAL;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Repository
{
    public class BookTitleRepository(BookWormDbContext context) : GenericRepository<BookTitle>(context), IBookTitleRepository
    {
        public async Task<BookTitle?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(b => b.Author)
                .Include(b => b.Language)
                .Include(b => b.BookQuotes)
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}