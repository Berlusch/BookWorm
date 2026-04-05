using AutoMapper;
using BookWorm.Common;
using BookWorm.Common.Factories;
using BookWorm.Service.Common;
using BookWorm.WebApi.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BookWorm.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BookTitleController : ControllerBase
    {
        private readonly IBookTitleService _bookTitleService;
        private readonly IMapper _mapper;
        public BookTitleController(IBookTitleService bookTitleService, IMapper mapper)
        {
            _bookTitleService = bookTitleService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<PagedResult<BookTitleReadDto>>> GetBookTitles(
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
            var pagedBookTitles = await _bookTitleService.GetBookTitlesAsync(paging, sorting, filterParams);
            var dto = new PagedResult<BookTitleReadDto>
            {
                Items = pagedBookTitles.Items.Select(b => _mapper.Map<BookTitleReadDto>(b)).ToList(),
                TotalCount = pagedBookTitles.TotalCount,
                Paging = pagedBookTitles.Paging
            };
            return Ok(dto);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BookTitleReadDto>> GetBookTitleById(int id)
        {
            var bookTitle = await _bookTitleService.GetByIdWithDetailsAsync(id);
            if (bookTitle == null)
                return NotFound();
            var dto = _mapper.Map<BookTitleReadDto>(bookTitle);
            return Ok(dto);
        }
        [HttpPost]
        public async Task<ActionResult<BookTitleReadDto>> AddBookTitle([FromBody] BookTitleInsertUpdateDto dto)
        {
            var bookTitle = _mapper.Map<BookTitle>(dto);
            var added = await _bookTitleService.AddBookTitleAsync(bookTitle, dto.GenreIds);
            var readDto = _mapper.Map<BookTitleReadDto>(added);
            return CreatedAtAction(nameof(GetBookTitleById), new { id = readDto.Id }, readDto);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<BookTitleReadDto>> UpdateBookTitle(int id, [FromBody] BookTitleInsertUpdateDto dto)
        {
            var bookTitle = _mapper.Map<BookTitle>(dto);
            var updated = await _bookTitleService.UpdateBookTitleAsync(id, bookTitle, dto.GenreIds);
            if (updated == null)
                return NotFound();
            var readDto = _mapper.Map<BookTitleReadDto>(updated);
            return Ok(readDto);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBookTitle(int id)
        {
            var deleted = await _bookTitleService.DeleteBookTitleAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}