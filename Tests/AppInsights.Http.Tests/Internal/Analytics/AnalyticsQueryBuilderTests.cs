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
            Assert.Equal($"{AnalyticsSchema.AvailabilityResults} | where filter1 == 'value1'", query);
        }

        [Fact]
        public void Should_BuildQueryWithMultipleFilter()
        {
            var queryBuilder = new AnalyticsQueryBuilder(AnalyticsSchema.AvailabilityResults)
                .WithFilter("filter1", AnalyticFilterOperator.Equal, "value1")
                .WithFilter("filter2", AnalyticFilterOperator.NotEqual, "value2");

            var query = queryBuilder.ToString();
            Assert.Equal($"{AnalyticsSchema.AvailabilityResults} | where filter1 == 'value1' | where filter2 != 'value2'", query);
        }

        [Fact]
        public void Should_BuildQueryWithFilterMultipleValues()
        {
            var queryBuilder = new AnalyticsQueryBuilder(AnalyticsSchema.AvailabilityResults)
                .WithFilter("filter1", AnalyticFilterOperator.In, "value1", "value2");

            var query = queryBuilder.ToString();
            Assert.Equal($"{AnalyticsSchema.AvailabilityResults} | where filter1 in ('value1', 'value2')", query);
        }

        [Fact]
        public void Should_BuildQueryWithFilterMultipleValues_OneParameter()
        {
            var queryBuilder = new AnalyticsQueryBuilder(AnalyticsSchema.AvailabilityResults)
                .WithFilter("filter1", AnalyticFilterOperator.In, "value1");

            var query = queryBuilder.ToString();
            Assert.Equal($"{AnalyticsSchema.AvailabilityResults} | where filter1 in ('value1')", query);
        }

        [Fact]
        public void Should_BuildQueryWithSummarizeCount()
        {
            var queryBuilder = new AnalyticsQueryBuilder(AnalyticsSchema.AvailabilityResults)
                .WithFilter("filter1", AnalyticFilterOperator.Equal, "value1")
                .WithSummarizeCount("name");

            var query = queryBuilder.ToString();
            Assert.Equal($"{AnalyticsSchema.AvailabilityResults} | where filter1 == 'value1' | summarize count() by name", query);
        }

        [Fact]
        public void Should_BuildQueryWithSummarizeCount_MultipleFields()
        {
            var queryBuilder = new AnalyticsQueryBuilder(AnalyticsSchema.AvailabilityResults)
                .WithFilter("filter1", AnalyticFilterOperator.Equal, "value1")
                .WithSummarizeCount("name", "role");

            var query = queryBuilder.ToString();
            Assert.Equal($"{AnalyticsSchema.AvailabilityResults} | where filter1 == 'value1' | summarize count() by name, role", query);
        }

        [Fact]
        public void Should_BuildQueryWithProject()
        {
            var queryBuilder = new AnalyticsQueryBuilder(AnalyticsSchema.AvailabilityResults)
                .WithProject("field1", "field2");

            var query = queryBuilder.ToString();
            Assert.Equal($"{AnalyticsSchema.AvailabilityResults} | project field1, field2", query);
        }

        [Fact]
        public void Should_BuildQueryWithTimestamp_Ago()
        {
            var queryBuilder = new AnalyticsQueryBuilder(AnalyticsSchema.AvailabilityResults)
                .WithTimestampFilter(AnalyticFilterOperator.SuperiorOrEqual, AnalyticTimestampFilterOperator.Ago, AnalyticTimestampDuration.HalfDay);

            var query = queryBuilder.ToString();
            Assert.Equal($"{AnalyticsSchema.AvailabilityResults} | where timestamp >= ago(12h)", query);
        }

        [Fact]
        public void Should_BuildQuery_WithTop()
        {
            const int top = 10;
            const string byField = "field1";
            var queryBuilder = new AnalyticsQueryBuilder(AnalyticsSchema.AvailabilityResults)
                .WithTop(top, byField);

            var query = queryBuilder.ToString();
            Assert.Equal($"{AnalyticsSchema.AvailabilityResults} | top {top} by {byField}", query);
        }

        [Fact]
        public void Should_BuildQuery_WithTop_AndTopOrdering()
        {
            const int top = 10;
            const string byField = "field1";
            var queryBuilder = new AnalyticsQueryBuilder(AnalyticsSchema.AvailabilityResults)
                .WithTop(top, byField, AnalyticTopOrderingOperator.Asc);

            var query = queryBuilder.ToString();
            Assert.Equal($"{AnalyticsSchema.AvailabilityResults} | top {top} by {byField} asc", query);
        }

        [Fact]
        public void Should_BuildQuery_WithTop_AndNullsOrdering()
        {
            const int top = 10;
            const string byField = "field1";
            var queryBuilder = new AnalyticsQueryBuilder(AnalyticsSchema.AvailabilityResults)
                .WithTop(top, byField, AnalyticNullsOrderingOperator.NullsLast);

            var query = queryBuilder.ToString();
            Assert.Equal($"{AnalyticsSchema.AvailabilityResults} | top {top} by {byField} nulls last", query);
        }

        [Fact]
        public void Should_BuildQuery_WithTop_AndTopOrdering_AndNullsOrdering()
        {
            const int top = 10;
            const string byField = "field1";
            var queryBuilder = new AnalyticsQueryBuilder(AnalyticsSchema.AvailabilityResults)
                .WithTop(top, byField, AnalyticTopOrderingOperator.Asc, AnalyticNullsOrderingOperator.NullsFirst);

            var query = queryBuilder.ToString();
            Assert.Equal($"{AnalyticsSchema.AvailabilityResults} | top {top} by {byField} asc nulls first", query);
        }
    }
}