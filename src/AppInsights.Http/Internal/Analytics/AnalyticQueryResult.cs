using AppInsights.Http.Internal.Analytics;
using System.Collections.Generic;
using System.Linq;

namespace AppInsights.Http.Analytics.Internals
{
    internal class AnalyticQueryResult : IAnalyticQueryResult
    {
        private readonly AppInsightsQueryResult _queryResult;

        public IEnumerable<IAnalyticQueryResultRow> Rows => _queryResult.Tables.First().Rows.Select((row) =>
        {
            var rowDictionary = new Dictionary<string, string>();
            for(int i = 0; i < row.Length; i++)
            {
                rowDictionary.Add(_queryResult.Tables.First().Columns[i].Name, row[i]);
            }
            return new AnalyticQueryResultRow(rowDictionary);
        });
        
        public AnalyticQueryResult(AppInsightsQueryResult queryResult) => _queryResult = queryResult;
    }
}
