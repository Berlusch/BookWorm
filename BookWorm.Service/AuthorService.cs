using Bookworm.Repository.Common;
using BookWorm.Common;
using BookWorm.Model;
using BookWorm.Service.Common;
using Microsoft.EntityFrameworkCore;

namespace BookWorm.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<PagedResult<Author>> GetAuthorsAsync(PFSParameters pfs)
        {
            var query = _authorRepository.GetQuery(pfs);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip(pfs.Paging.Skip)
                .Take(pfs.Paging.PageSize)
                .ToListAsync();

            return new PagedResult<Author>
            {
                Items = items.ToList(),       
                TotalCount = totalCount,
                Paging = pfs.Paging           
            };
        }

        public async Task<Author> GetAuthorByIdAsync(int id)
        {
            return await _authorRepository.GetByIdAsync(id);
        }

        public async Task<Author> AddAuthorAsync(Author author)
        {
            return await _authorRepository.AddAsync(author);
        }

        public async Task<Author> UpdateAuthorAsync(int id, Author author)
        {
            var existing = await _authorRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Author with ID {id} not found.");

            // Update polja
            existing.FirstName = author.FirstName;
            existing.LastName = author.LastName;
            existing.BirthYear = author.BirthYear;
            existing.DeathYear = author.DeathYear;
            existing.Biography = author.Biography;
            existing.NationalLiterature = author.NationalLiterature;

            return await _authorRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            return await _authorRepository.DeleteAsync(id);
        }
    }
}