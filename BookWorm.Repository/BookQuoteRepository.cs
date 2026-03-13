using Bookworm.Repository.Common;
using BookWorm.DAL;
using BookWorm.Model;

namespace Bookworm.Repository
{
    public class BookQuoteRepository(BookWormDbContext context) : GenericRepository<BookQuote>(context), IBookQuoteRepository
    {

    }
}