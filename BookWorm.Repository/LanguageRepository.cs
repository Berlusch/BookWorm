using Bookworm.Repository.Common;
using BookWorm.DAL;
using BookWorm.Model;

namespace Bookworm.Repository
{
    public class LanguageRepository(BookWormDbContext context) : GenericRepository<Language>(context), ILanguageRepository
    {

    }
}