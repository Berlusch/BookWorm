using BookWorm.Common;
using BookWorm.Model;
namespace BookWorm.Service.Common
{
    public interface IAuthorService
    {
        Task<PagedResult<Author>> GetAuthorsAsync(PagingParameters paging, SortingParameters sorting, FilterParameters filter);

        Task<Author> GetAuthorByIdAsync(int id);

        Task<Author> AddAuthorAsync(Author author);

        Task<Author> UpdateAuthorAsync(int id, Author author);

        Task<bool> DeleteAuthorAsync(int id);
    }
}