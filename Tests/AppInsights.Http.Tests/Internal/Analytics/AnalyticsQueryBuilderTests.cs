using AppInsights.Http.Analytics;
using AppInsights.Http.Internal.Analytics;
using Xunit;

namespace AppInsights.Http.Tests.Internal.Analytics
{
    public class AnalyticsQueryBuilderTests
    {
        [Fact]
        public void Should_BuildQueryWithSchemaOnly()
        {
            var queryBuilder = new AnalyticsQueryBuilder(AnalyticsSchema.AvailabilityResults);
            var query = queryBuilder.ToString();
            Assert.Equal(query, AnalyticsSchema.AvailabilityResults.ToString());
        }

        [Fact]
        public void Should_BuildQueryWithOneFilter()
        {
            var queryBuilder = new AnalyticsQueryBuilder(AnalyticsSchema.AvailabilityResults)
                .WithFilter("filter1", AnalyticFilterOperator.Equal, "value1");
            var query = queryBuilder.ToString();
            Assert.Equal(query, $"{AnalyticsSchema.AvailabilityResults} | where filter1 == 'value1'");
        }

        [Fact]
        public void Should_BuildQueryWithMultipleFilter()
        {
            var queryBuilder = new AnalyticsQueryBuilder(AnalyticsSchema.AvailabilityResults)
                .WithFilter("filter1", AnalyticFilterOperator.Equal, "value1")
                .WithFilter("filter2", AnalyticFilterOperator.NotEqual, "value2");

            var query = queryBuilder.ToString();
            Assert.Equal(query, $"{AnalyticsSchema.AvailabilityResults} | where filter1 == 'value1' | where filter2 != 'value2'");
        }
    }
}
