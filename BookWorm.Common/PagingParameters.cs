namespace BookWorm.Common
{
    public class PagingParameters
    {
        private int _pageNumber = 1;
        private int _pageSize = 5;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 5 : value;
        }

        public int Skip => (PageNumber - 1) * PageSize;

        public static PagingParameters Lookup(int pageSize = 1000)
        {
            return new PagingParameters
            {
                PageNumber = 1,
                PageSize = pageSize
            };
        }
    }
}
