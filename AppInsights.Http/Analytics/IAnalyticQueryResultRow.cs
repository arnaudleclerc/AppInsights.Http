using System.Collections.Generic;

namespace AppInsights.Http.Analytics
{
    /// <summary>
    /// Row of the result of a request to application insights
    /// </summary>
    public interface IAnalyticQueryResultRow
    {
        /// <summary>
        /// Columns of the row
        /// </summary>
        /// <value></value>
        IReadOnlyDictionary<string, string> Columns { get; }
    }
}
