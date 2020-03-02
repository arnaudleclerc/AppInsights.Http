namespace AppInsights.Http.Analytics
{
    /// <summary>
    /// Exposes methods to create query builders
    /// </summary>
    public interface IAnalyticsQueryBuilderFactory
    {
        /// <summary>
        /// Create a query builder on the given application insights schema
        /// </summary>
        /// <param name="schema">Schema to query</param>
        /// <returns>Query builder</returns>
        IAnalyticsQueryBuilder CreateAnalyticsQueryBuilder(AnalyticsSchema schema);
    }
}
