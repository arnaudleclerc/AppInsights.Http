using AppInsights.Http.Configuration;
using AppInsights.Http.Exceptions;
using AppInsights.Http.Internal.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AppInsights.Http.Tests.Internal.Http
{
    public class AppInsightsHttpClientTests
    {
        public class HttpClientMockHandler : DelegatingHandler
        {
            private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handler;
            private readonly Func<HttpRequestMessage, bool> _expectation;
            public HttpClientMockHandler(Func<HttpResponseMessage> responseFunc, Func<HttpRequestMessage, bool> expectation)
            {
                _handler = (request, cancellationToken) => Task.FromResult(responseFunc());
                _expectation = expectation;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if(!_expectation(request))
                {
                    throw new NotImplementedException();
                }
                return _handler(request, cancellationToken);
            }
        }

        [Fact]
        public async Task Should_ThrowException_ResultIsNoSuccess()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var options = new Mock<IOptions<AppInsightsConfiguration>>();
            var logger = new Mock<ILogger>();
            
            var appInsightsConfiguration = new AppInsightsConfiguration
            {
                APIKey = "apikey",
                ApplicationId = "appid"
            };
            options.Setup(o => o.Value).Returns(appInsightsConfiguration);

            var metrics = Metrics.AvailabilityResultsAvailabilityPercentage;

            var httpClientHandler = new HttpClientMockHandler(() => new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("{'error': {'message': 'The requested path does not exist', 'code': 'PathNotFoundError'}}")
            }, hrm => hrm.Method == HttpMethod.Get
                && hrm.RequestUri.ToString() == $"https://api.applicationinsights.io/v1/apps/{appInsightsConfiguration.ApplicationId}/metrics/{metrics.ToString()}"
                && hrm.Headers.Contains("x-api-key")
                && hrm.Headers.GetValues("x-api-key").First() == appInsightsConfiguration.APIKey);
            var httpClient = new Mock<HttpClient>(httpClientHandler);
            httpClientFactory.Setup(hcf => hcf.CreateClient(It.IsAny<string>())).Returns(httpClient.Object);

            var appInsightsHttpClient = new AppInsightsHttpClient(httpClientFactory.Object,
                options.Object,
                logger.Object);

            await Assert.ThrowsAsync<AppInsightsException>(async() => await appInsightsHttpClient.GetMetricAsync(metrics));

            httpClientFactory.Verify(hcf => hcf.CreateClient(appInsightsConfiguration.APIKey), Times.Once);
        }

        [Fact]
        public async Task Should_ReturnMetric()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var options = new Mock<IOptions<AppInsightsConfiguration>>();
            var logger = new Mock<ILogger>();

            var appInsightsConfiguration = new AppInsightsConfiguration
            {
                APIKey = "apikey",
                ApplicationId = "appid"
            };
            options.Setup(o => o.Value).Returns(appInsightsConfiguration);

            var metrics = Metrics.RequestsCount;

            var httpClientHandler = new HttpClientMockHandler(() => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{'value': {'start': '2019-07-08T18:29:28.972Z','end': '2019-07-09T06:29:28.972Z','requests/count': {'sum': 367}}}")
            }, hrm => hrm.Method == HttpMethod.Get
                && hrm.RequestUri.ToString() == $"https://api.applicationinsights.io/v1/apps/{appInsightsConfiguration.ApplicationId}/metrics/{metrics.ToString()}"
                && hrm.Headers.Contains("x-api-key")
                && hrm.Headers.GetValues("x-api-key").First() == appInsightsConfiguration.APIKey);;
            var httpClient = new Mock<HttpClient>(httpClientHandler);
            httpClientFactory.Setup(hcf => hcf.CreateClient(It.IsAny<string>())).Returns(httpClient.Object);

            var appInsightsHttpClient = new AppInsightsHttpClient(httpClientFactory.Object,
                options.Object,
                logger.Object);

            var result = await appInsightsHttpClient.GetMetricAsync(metrics);

            Assert.Equal(367, result.Aggregation.Sum);

            httpClientFactory.Verify(hcf => hcf.CreateClient(appInsightsConfiguration.APIKey), Times.Once);
        }
    }
}
