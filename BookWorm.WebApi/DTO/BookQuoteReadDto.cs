namespace BookWorm.WebApi.DTO
{
    public class BookQuoteReadDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public int BookTitleId { get; set; }
        public string BookTitleName { get; set; } = null!;
    }
}