namespace BookWorm.Model.Common
{
    public interface IGenre : IEntityBase
    {
        string Name { get; set; }
        string Description { get; set; }
        
    }
}
