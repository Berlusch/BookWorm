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
    public class BookQuoteController : ControllerBase
    {
        private readonly IBookQuoteService _bookQuoteService;
        private readonly IMapper _mapper;

        public BookQuoteController(IBookQuoteService bookQuoteService, IMapper mapper)
        {
            _bookQuoteService = bookQuoteService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<BookQuoteReadDto>>> GetBookQuotes(
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

            var pagedQuotes = await _bookQuoteService.GetBookQuotesAsync(paging, sorting, filterParams);
            var dto = new PagedResult<BookQuoteReadDto>
            {
                Items = pagedQuotes.Items.Select(q => _mapper.Map<BookQuoteReadDto>(q)).ToList(),
                TotalCount = pagedQuotes.TotalCount,
                Paging = pagedQuotes.Paging
            };
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookQuoteReadDto>> GetBookQuoteById(int id)
        {
            var bookQuote = await _bookQuoteService.GetBookQuoteByIdAsync(id);
            if (bookQuote == null)
                return NotFound();
            var dto = _mapper.Map<BookQuoteReadDto>(bookQuote);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<BookQuoteReadDto>> AddBookQuote([FromBody] BookQuoteInsertUpdateDto dto)
        {
            var bookQuote = _mapper.Map<BookQuote>(dto);
            var added = await _bookQuoteService.AddBookQuoteAsync(bookQuote);
            var readDto = _mapper.Map<BookQuoteReadDto>(added);
            return CreatedAtAction(nameof(GetBookQuoteById), new { id = readDto.Id }, readDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookQuoteReadDto>> UpdateBookQuote(int id, [FromBody] BookQuoteInsertUpdateDto dto)
        {
            var bookQuote = _mapper.Map<BookQuote>(dto);
            var updated = await _bookQuoteService.UpdateBookQuoteAsync(id, bookQuote);
            if (updated == null)
                return NotFound();
            var readDto = _mapper.Map<BookQuoteReadDto>(updated);
            return Ok(readDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBookQuote(int id)
        {
            var deleted = await _bookQuoteService.DeleteBookQuoteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}