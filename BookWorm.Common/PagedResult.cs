namespace BookWorm.Common
{
    public class PagedResult<T>
    {
        public required IReadOnlyList<T> Items { get; init; }
        public required int TotalCount { get; init; }
        public required PagingParameters Paging { get; init; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / Paging.PageSize);
    }
}
