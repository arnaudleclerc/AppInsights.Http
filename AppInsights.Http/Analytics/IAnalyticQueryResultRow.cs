using System.Collections.Generic;

namespace AppInsights.Http.Analytics
{
    public interface IAnalyticQueryResultRow
    {
        IReadOnlyDictionary<string, string> Columns { get; }
    }
}
