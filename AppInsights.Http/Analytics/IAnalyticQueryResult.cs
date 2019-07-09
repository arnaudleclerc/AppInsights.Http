using System.Collections.Generic;

namespace AppInsights.Http.Analytics
{
    public interface IAnalyticQueryResult
    {
        IEnumerable<IAnalyticQueryResultRow> Rows { get; }
    }
}
