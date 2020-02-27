using System.Collections.Generic;

namespace AppInsights.Http.Analytics
{
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
