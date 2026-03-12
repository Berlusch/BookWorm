using Bookworm.Repository.Common;
using BookWorm.Common;
using BookWorm.Model;
using BookWorm.Service.Common;
using Microsoft.EntityFrameworkCore;

namespace BookWorm.Service
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;
        public LanguageService(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }
        public async Task<PagedResult<Language>> GetLanguagesAsync(PagingParameters paging, SortingParameters sorting, FilterParameters filter)
        {
            var query = _languageRepository.GetQuery(paging, sorting, filter);
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip(paging.Skip)
                .Take(paging.PageSize)
                .ToListAsync();
            return new PagedResult<Language>
            {
                Items = items.ToList(),
                TotalCount = totalCount,
                Paging = paging
            };
        }
        public async Task<Language> GetLanguageByIdAsync(int id)
        {
            return await _languageRepository.GetByIdAsync(id);
        }
        public async Task<Language> AddLanguageAsync(Language language)
        {
            return await _languageRepository.AddAsync(language);
        }
        public async Task<Language> UpdateLanguageAsync(int id, Language language)
        {
            var existing = await _languageRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Language with ID {id} not found.");
            existing.Name = language.Name;
            return await _languageRepository.UpdateAsync(existing);
        }
        public async Task<bool> DeleteLanguageAsync(int id)
        {
            return await _languageRepository.DeleteAsync(id);
        }
    }
}