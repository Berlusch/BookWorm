namespace BookWorm.WebApi.DTO
{
    public class BookQuoteInsertUpdateDto
    {
        public string Text { get; set; } = null!;
        public int BookTitleId { get; set; }
    }
}