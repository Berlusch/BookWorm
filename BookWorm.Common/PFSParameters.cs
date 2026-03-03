
namespace BookWorm.Common
{
    public class PFSParameters
    {
        public PagingParameters Paging { get; set; } = new();
        public SortingParameters Sorting { get; set; } = new();
        public FilterParameters Filter { get; set; } = new();
    }
}
