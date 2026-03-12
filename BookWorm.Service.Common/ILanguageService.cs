using BookWorm.Common;
using BookWorm.Model;

namespace BookWorm.Service.Common
{
    public interface ILanguageService
    {
        Task<PagedResult<Language>> GetLanguagesAsync(PagingParameters paging, SortingParameters sorting, FilterParameters filter);
        Task<Language> GetLanguageByIdAsync(int id);
        Task<Language> AddLanguageAsync(Language language);
        Task<Language> UpdateLanguageAsync(int id, Language language);
        Task<bool> DeleteLanguageAsync(int id);
    }
}