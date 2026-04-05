using Bookworm.Repository.Common;
using BookWorm.DAL;
using BookWorm.Model;
using Microsoft.EntityFrameworkCore;

namespace Bookworm.Repository
{
    public class AuthorRepository(BookWormDbContext context) : GenericRepository<Author>(context), IAuthorRepository
    {
        public async Task<Author?> GetByFullNameAsync(string fullName)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.FullName == fullName);
        }
        public async Task<Author?> GetByIdWithBooksAsync(int id)
        {
            return await _dbSet
                .Include(a => a.BookTitles)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
