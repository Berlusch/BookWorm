using Bookworm.Repository.Common;
using BookWorm.Common;
using BookWorm.Model;
using BookWorm.Service.Common;
using Microsoft.EntityFrameworkCore;
namespace BookWorm.Service
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }
        public async Task<PagedResult<Genre>> GetGenresAsync(PagingParameters paging, SortingParameters sorting, FilterParameters filter)
        {
            var query = _genreRepository.GetQuery(paging, sorting, filter);
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip(paging.Skip)
                .Take(paging.PageSize)
                .ToListAsync();
            return new PagedResult<Genre>
            {
                Items = items.ToList(),
                TotalCount = totalCount,
                Paging = paging
            };
        }
        public async Task<Genre> GetGenreByIdAsync(int id)
        {
            return await _genreRepository.GetByIdAsync(id);
        }
        public async Task<Genre> AddGenreAsync(Genre genre)
        {
            return await _genreRepository.AddAsync(genre);
        }
        public async Task<Genre> UpdateGenreAsync(int id, Genre genre)
        {
            var existing = await _genreRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Genre with ID {id} not found.");
            existing.Name = genre.Name;
            existing.Description = genre.Description;
            return await _genreRepository.UpdateAsync(existing);
        }
        public async Task<bool> DeleteGenreAsync(int id)
        {
            return await _genreRepository.DeleteAsync(id);
        }
    }
}