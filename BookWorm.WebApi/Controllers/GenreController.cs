using AutoMapper;
using BookWorm.Common;
using BookWorm.Common.Factories;
using BookWorm.Model;
using BookWorm.Service.Common;
using BookWorm.WebApi.DTO;
using Microsoft.AspNetCore.Mvc;
namespace BookWorm.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        private readonly IMapper _mapper;
        public GenreController(IGenreService genreService, IMapper mapper)
        {
            _genreService = genreService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<PagedResult<GenreReadDto>>> GetGenres(
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
            var pagedGenres = await _genreService.GetGenresAsync(paging, sorting, filterParams);
            var dto = new PagedResult<GenreReadDto>
            {
                Items = pagedGenres.Items.Select(g => _mapper.Map<GenreReadDto>(g)).ToList(),
                TotalCount = pagedGenres.TotalCount,
                Paging = pagedGenres.Paging
            };
            return Ok(dto);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreReadDto>> GetGenreById(int id)
        {
            var genre = await _genreService.GetGenreByIdAsync(id);
            if (genre == null)
                return NotFound();
            var dto = _mapper.Map<GenreReadDto>(genre);
            return Ok(dto);
        }
        [HttpPost]
        public async Task<ActionResult<GenreReadDto>> AddGenre([FromBody] GenreInsertUpdateDto dto)
        {
            var genre = _mapper.Map<Genre>(dto);
            var added = await _genreService.AddGenreAsync(genre);
            var readDto = _mapper.Map<GenreReadDto>(added);
            return CreatedAtAction(nameof(GetGenreById), new { id = readDto.Id }, readDto);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<GenreReadDto>> UpdateGenre(int id, [FromBody] GenreInsertUpdateDto dto)
        {
            var genre = _mapper.Map<Genre>(dto);
            var updated = await _genreService.UpdateGenreAsync(id, genre);
            if (updated == null)
                return NotFound();
            var readDto = _mapper.Map<GenreReadDto>(updated);
            return Ok(readDto);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGenre(int id)
        {
            var deleted = await _genreService.DeleteGenreAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}