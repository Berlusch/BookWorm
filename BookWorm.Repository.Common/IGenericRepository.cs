using BookWorm.Common;

namespace BookWorm.Repository.Common
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetQuery(PagingParameters paging, SortingParameters sorting, FilterParameters filter);
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}