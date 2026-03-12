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
        public async Task<PagedResult<Author>> GetAuthorsAsync(PagingParameters paging, SortingParameters sorting, FilterParameters filter)
        {
            var query = _authorRepository.GetQuery(paging, sorting, filter);
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip(paging.Skip)
                .Take(paging.PageSize)
                .ToListAsync();
            return new PagedResult<Author>
            {
                Items = items.ToList(),
                TotalCount = totalCount,
                Paging = paging
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