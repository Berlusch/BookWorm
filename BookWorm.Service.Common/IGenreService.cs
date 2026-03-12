using BookWorm.Common;
using BookWorm.Model;

namespace BookWorm.Service.Common
{
    public interface IGenreService
    {
        Task<PagedResult<Genre>> GetGenresAsync(PagingParameters paging, SortingParameters sorting, FilterParameters filter);
        Task<Genre> GetGenreByIdAsync(int id);
        Task<Genre> AddGenreAsync(Genre genre);
        Task<Genre> UpdateGenreAsync(int id, Genre genre);
        Task<bool> DeleteGenreAsync(int id);
    }
}