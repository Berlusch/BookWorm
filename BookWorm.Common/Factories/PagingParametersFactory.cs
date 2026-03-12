namespace BookWorm.Common.Factories
{
    public static class PagingParametersFactory
    {
        public static PagingParameters Create(int pageNumber = 1, int pageSize = 5)
        {
            return new PagingParameters
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public static PagingParameters CreateLookup(int pageSize = 1000)
        {
            return PagingParameters.Lookup(pageSize);
        }
    }
}

