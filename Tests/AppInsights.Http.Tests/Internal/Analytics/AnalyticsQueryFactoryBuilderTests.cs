namespace AppInsights.Http.Tests.Internal.Analytics
{
    using AppInsights.Http.Analytics;
    using AppInsights.Http.Internal.Analytics;
    using Xunit;
    
    public class AnalyticsQueryFactoryBuilderTests
    {
        [Fact]
        public void Should_CreateQueryBuilder()
        {
            var factory = new AnalyticsQueryBuilderFactory();
            var builder = factory.CreateAnalyticsQueryBuilder(AnalyticsSchema.AvailabilityResults);
            Assert.Equal(AnalyticsSchema.AvailabilityResults, builder.Schema);
        }
    }
}
