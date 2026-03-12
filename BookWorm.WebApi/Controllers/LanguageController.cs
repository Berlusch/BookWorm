using AutoMapper;
using BookWorm.Common;
using BookWorm.Common.Factories;
using BookWorm.Service.Common;
using BookWorm.WebApi.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BookWorm.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _languageService;
        private readonly IMapper _mapper;
        public LanguageController(ILanguageService languageService, IMapper mapper)
        {
            _languageService = languageService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<PagedResult<LanguageReadDto>>> GetLanguages(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string orderBy = "Id",
            [FromQuery] bool descending = false,
            [FromQuery] string filterProperty = "",
            [FromQuery] string filter = "")
        {
            var paging = PagingParametersFactory.Create(pageNumber, pageSize);
            var sorting = SortingParametersFactory.Create(orderBy, descending);
            var filterParams = FilterParametersFactory.Create(filterProperty, filter);
            var pagedLanguages = await _languageService.GetLanguagesAsync(paging, sorting, filterParams);
            var dto = new PagedResult<LanguageReadDto>
            {
                Items = pagedLanguages.Items.Select(l => _mapper.Map<LanguageReadDto>(l)).ToList(),
                TotalCount = pagedLanguages.TotalCount,
                Paging = pagedLanguages.Paging
            };
            return Ok(dto);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<LanguageReadDto>> GetLanguageById(int id)
        {
            var language = await _languageService.GetLanguageByIdAsync(id);
            if (language == null)
                return NotFound();
            var dto = _mapper.Map<LanguageReadDto>(language);
            return Ok(dto);
        }
    }
}