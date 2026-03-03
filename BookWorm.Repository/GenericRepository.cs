using BookWorm.Repository;
using BookWorm.Repository.Common;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using BookWorm.DAL;
using BookWorm.Common;


namespace Bookworm.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly BookWormDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public readonly IUnitOfWork UnitOfWork;

        public GenericRepository(BookWormDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
            UnitOfWork = new UnitOfWork(_context);
        }

        public IQueryable<T> GetQuery(PFSParameters? parameters = null)
        {
            parameters ??= new PFSParameters();

            IQueryable<T> query = _dbSet.AsQueryable();

            // FILTER
            if (!string.IsNullOrEmpty(parameters.Filter.Filter) &&
                !string.IsNullOrEmpty(parameters.Filter.PropertyName))
            {
                var propInfo = typeof(T).GetProperty(parameters.Filter.PropertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propInfo != null && propInfo.PropertyType == typeof(string))
                {
                    query = query.Where(e =>
                        EF.Property<string>(e, parameters.Filter.PropertyName)
                          .Contains(parameters.Filter.Filter));
                }
            }

            // SORT
            if (!string.IsNullOrEmpty(parameters.Sorting.OrderBy))
            {
                var propInfo = typeof(T).GetProperty(parameters.Sorting.OrderBy,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propInfo != null)
                {
                    query = parameters.Sorting.Descending
                        ? query.OrderByDescending(e => EF.Property<object>(e, propInfo.Name))
                        : query.OrderBy(e => EF.Property<object>(e, propInfo.Name));
                }
            }

            return query;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id)
                ?? throw new KeyNotFoundException($"Entity with ID {id} not found.");
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await UnitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await UnitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await UnitOfWork.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
