namespace BookWorm.Common.Factories
{
    public static class FilterParametersFactory
    {
        public static FilterParameters Create(string propertyName = "", string filter = "")
        {
            return new FilterParameters
            {
                PropertyName = propertyName,
                Filter = filter
            };
        }
    }
}
