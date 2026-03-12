using BookWorm.Model.Common;

namespace BookWorm.Model
{
    public class BookQuote : EntityBase, IBookQuote
    {
        public string Text { get; set; } = null!;

        // FK and navigation 1:1 to BookTitle
        public int BookTitleId { get; set; }        
        public BookTitle BookTitle { get; set; } = null!;  
    }
}
