using BookWorm.Model;
using BookWorm.Repository.Common;

namespace Bookworm.Repository.Common
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        Task<Author?> GetByIdWithBooksAsync(int id);
    }
}
