using BookWorm.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookWorm.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookTitleController : ControllerBase
    {
        private readonly BookWormDbContext _dbContext;

        public BookTitleController(BookWormDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _dbContext.BookTitles
                .Include(b => b.Author)
                .Include(b => b.Language)
                .Include(b => b.TagLine)
                .Include(b => b.Genres)
                .ToListAsync();

            return Ok(books);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _dbContext.BookTitles
                .Include(b => b.Author)
                .Include(b => b.Language)
                .Include(b => b.TagLine)
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            return Ok(book);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(BookTitle book)
        {
            _dbContext.BookTitles.Add(book);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }
    }
}
