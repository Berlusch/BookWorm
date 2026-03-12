using BookWorm.Repository.Common;

namespace Bookworm.Repository.Common
{
    public interface IBookTitleRepository : IGenericRepository<BookTitle>
    {
        Task<BookTitle?> GetByIdWithDetailsAsync(int id);
    }
}