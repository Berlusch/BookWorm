namespace BookWorm.Model.Common
{
    public interface IBookTitle
    {
        string Title { get; }              
        string? Subtitle { get; }          
        string AuthorName { get; set; }    
        string LanguageName { get; set; }  
        string TagLineText { get; set; }   
    }
}
