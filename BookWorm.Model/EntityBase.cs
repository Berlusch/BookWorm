using BookWorm.Model.Common;

namespace BookWorm.Model
{
    public abstract class EntityBase: IEntityBase
    {
        public int Id { get; set; }
    }
}
