namespace AppInsights.Http.Analytics
{
    using System.Collections.Generic;

    /// <summary>
    /// Result of a request to application insights
    /// </summary>
    public interface IAnalyticQueryResult
    {
        /// <summary>
        /// Rows returned by the request
        /// </summary>
        /// <value></value>
        IEnumerable<IAnalyticQueryResultRow> Rows { get; }
    }
}
