using BookWorm.Repository.Common;
using BookWorm.DAL;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookWorm.Repository
{
    public sealed class UnitOfWork(BookWormDbContext context) : IUnitOfWork
    {
        private readonly BookWormDbContext _context =
            context ?? throw new ArgumentNullException(nameof(context));

        private IDbContextTransaction? _transaction;

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}

