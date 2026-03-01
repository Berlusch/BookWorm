namespace BookWorm.Model.Common
{
    public interface IAuthor : IEntityBase
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        int? BirthYear { get; set; }
        int? DeathYear { get; set; }
        string Biography { get; set; }
        string NationalLiterature { get; set; }
        
    }
}
