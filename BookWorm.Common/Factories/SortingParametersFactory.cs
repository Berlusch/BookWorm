namespace BookWorm.Common.Factories
{
    public static class SortingParametersFactory
    {
        public static SortingParameters Create(string orderBy = "Id", bool descending = false)
        {
            return new SortingParameters
            {
                OrderBy = orderBy,
                Descending = descending
            };
        }
    }
}