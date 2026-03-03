using AutoMapper;
using BookWorm.Common;
using BookWorm.Model;
using BookWorm.Service.Common;
using BookWorm.WebApi.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BookWorm.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;

        public AuthorController(IAuthorService authorService, IMapper mapper)
        {
            _authorService = authorService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<PagedResult<AuthorReadDto>>> GetAuthors([FromQuery] PFSParameters pfs)
        {
            var pagedAuthors = await _authorService.GetAuthorsAsync(pfs);

            // Mapiranje Author → AuthorReadDto
            var dto = new PagedResult<AuthorReadDto>
            {
                Items = pagedAuthors.Items.Select(a => _mapper.Map<AuthorReadDto>(a)).ToList(),
                TotalCount = pagedAuthors.TotalCount,
                Paging = pagedAuthors.Paging
            };

            return Ok(dto);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadDto>> GetAuthorById(int id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
                return NotFound();

            var dto = _mapper.Map<AuthorReadDto>(author);
            return Ok(dto);
        }
                
        [HttpPost]
        public async Task<ActionResult<AuthorReadDto>> AddAuthor([FromBody] AuthorInsertUpdateDto dto)
        {
            var author = _mapper.Map<Author>(dto);
            var added = await _authorService.AddAuthorAsync(author);

            var readDto = _mapper.Map<AuthorReadDto>(added);
            return CreatedAtAction(nameof(GetAuthorById), new { id = readDto.Id }, readDto);
        }
       
        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorReadDto>> UpdateAuthor(int id, [FromBody] AuthorInsertUpdateDto dto)
        {
            var author = _mapper.Map<Author>(dto);
            var updated = await _authorService.UpdateAuthorAsync(id, author);

            var readDto = _mapper.Map<AuthorReadDto>(updated);
            return Ok(readDto);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuthor(int id)
        {
            var result = await _authorService.DeleteAuthorAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
