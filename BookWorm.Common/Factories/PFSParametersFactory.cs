namespace BookWorm.Common.Factories
{
    public static class PFSParametersFactory
    {
        public static PFSParameters Create(
            int pageNumber = 1,
            int pageSize = 5,
            string orderBy = "Id",
            bool descending = false,
            string filterProperty = "",
            string filter = "")
        {
            return new PFSParameters
            {
                Paging = PagingParametersFactory.Create(pageNumber, pageSize),
                Sorting = SortingParametersFactory.Create(orderBy, descending),
                Filter = FilterParametersFactory.Create(filterProperty, filter)
            };
        }

        public static PFSParameters CreateDefault()
        {
            return new PFSParameters
            {
                Paging = PagingParametersFactory.Create(),
                Sorting = SortingParametersFactory.Create(),
                Filter = FilterParametersFactory.Create()
            };
        }
    }
}
