using Bookworm.Repository.Common;
using BookWorm.DAL;
using BookWorm.Model;

namespace Bookworm.Repository
{
    public class GenreRepository(BookWormDbContext context) : GenericRepository<Genre>(context), IGenreRepository
    {

    }
}