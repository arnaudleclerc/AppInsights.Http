using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AppInsights.Http.Configuration;
using AppInsights.Http.Exceptions;
using AppInsights.Http.Internal.Authentication;
using AppInsights.Http.Internal.Http;
using AppInsights.Http.Metrics;
using Microsoft.Extensions.Options;
using Moq;
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
                if (!_expectation(request))
                {
                    throw new NotImplementedException();
                }
                return _handler(request, cancellationToken);
            }
        }

        [Fact]
        public async Task GetMetricAsync_Should_ThrowException_ResultIsNoSuccess()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var options = new Mock<IOptions<AppInsightsConfiguration>>();
            var requestAuthenticator = new Mock<AppInsightsRequestAuthenticator>();
            requestAuthenticator.Setup(ra => ra.AddAuthenticationAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<AppInsightsConfiguration>())).Callback(
                (HttpRequestMessage request, AppInsightsConfiguration configuration) => {
                    request.Headers.Add("appinsights-mock-header", "appinsights-mock-header");
                }
            );

            var appInsightsConfiguration = new AppInsightsConfiguration
            {
                APIKey = "apikey",
                ApplicationId = "appid"
            };
            options.Setup(o => o.Value).Returns(appInsightsConfiguration);

            var metrics = MetricsDefinition.AvailabilityResultsAvailabilityPercentage;

            var httpClientHandler = new HttpClientMockHandler(() => new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("{'error': {'message': 'The requested path does not exist', 'code': 'PathNotFoundError'}}")
                }, hrm => hrm.Method == HttpMethod.Get &&
                hrm.RequestUri.ToString() == $"https://api.applicationinsights.io/v1/apps/{appInsightsConfiguration.ApplicationId}/metrics/{metrics}" &&
                hrm.Headers.Contains("appinsights-mock-header"));
            var httpClient = new Mock<HttpClient>(httpClientHandler);
            httpClientFactory.Setup(hcf => hcf.CreateClient(It.IsAny<string>())).Returns(httpClient.Object);

            var appInsightsHttpClient = new AppInsightsHttpClient(httpClientFactory.Object,
                options.Object,
                requestAuthenticator.Object);

            await Assert.ThrowsAsync<AppInsightsException>(async() => await appInsightsHttpClient.GetMetricAsync(metrics));

            httpClientFactory.Verify(hcf => hcf.CreateClient(appInsightsConfiguration.ApplicationId), Times.Once);
            requestAuthenticator.Verify(ra => ra.AddAuthenticationAsync(It.IsAny<HttpRequestMessage>(), appInsightsConfiguration), Times.Once);
            requestAuthenticator.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetMetricAsync_Should_ThrowException_ResultIsNoSuccess_MultipleConfigurations()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var requestAuthenticator = new Mock<AppInsightsRequestAuthenticator>();
            requestAuthenticator.Setup(ra => ra.AddAuthenticationAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<AppInsightsConfiguration>())).Callback(
                (HttpRequestMessage request, AppInsightsConfiguration configuration) => {
                    request.Headers.Add("appinsights-mock-header", "appinsights-mock-header");
                }
            );

            var appInsightsConfiguration = new AppInsightsConfiguration
            {
                APIKey = "apikey",
                ApplicationId = "appid"
            };

            var secondConfiguration = new AppInsightsConfiguration
            {
                APIKey = "secondapikey",
                ApplicationId = "secondappid"
            };
            var metrics = MetricsDefinition.AvailabilityResultsAvailabilityPercentage;

            var httpClientHandler = new HttpClientMockHandler(() => new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("{'error': {'message': 'The requested path does not exist', 'code': 'PathNotFoundError'}}")
                }, hrm => hrm.Method == HttpMethod.Get &&
                hrm.RequestUri.ToString() == $"https://api.applicationinsights.io/v1/apps/{appInsightsConfiguration.ApplicationId}/metrics/{metrics}" &&
                hrm.Headers.Contains("appinsights-mock-header"));
            var httpClient = new Mock<HttpClient>(httpClientHandler);
            httpClientFactory.Setup(hcf => hcf.CreateClient(It.IsAny<string>())).Returns(httpClient.Object);

            var appInsightsHttpClient = new AppInsightsHttpClient(httpClientFactory.Object,
                new [] { appInsightsConfiguration, secondConfiguration },
                requestAuthenticator.Object);

            await Assert.ThrowsAsync<AppInsightsException>(async() => await appInsightsHttpClient.GetMetricAsync(metrics));

            httpClientFactory.Verify(hcf => hcf.CreateClient(appInsightsConfiguration.ApplicationId), Times.Once);
            requestAuthenticator.Verify(ra => ra.AddAuthenticationAsync(It.IsAny<HttpRequestMessage>(), appInsightsConfiguration), Times.Once);
            requestAuthenticator.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetMetricAsync_Should_ReturnMetric()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var options = new Mock<IOptions<AppInsightsConfiguration>>();
            var requestAuthenticator = new Mock<AppInsightsRequestAuthenticator>();
            requestAuthenticator.Setup(ra => ra.AddAuthenticationAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<AppInsightsConfiguration>())).Callback(
                (HttpRequestMessage request, AppInsightsConfiguration configuration) => {
                    request.Headers.Add("appinsights-mock-header", "appinsights-mock-header");
                }
            );

            var appInsightsConfiguration = new AppInsightsConfiguration
            {
                APIKey = "apikey",
                ApplicationId = "appid"
            };
            options.Setup(o => o.Value).Returns(appInsightsConfiguration);

            var metrics = MetricsDefinition.RequestsCount;

            var httpClientHandler = new HttpClientMockHandler(() => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{'value': {'start': '2019-07-08T18:29:28.972Z','end': '2019-07-09T06:29:28.972Z','requests/count': {'sum': 367}}}")
                }, hrm => hrm.Method == HttpMethod.Get &&
                hrm.RequestUri.ToString() == $"https://api.applicationinsights.io/v1/apps/{appInsightsConfiguration.ApplicationId}/metrics/{metrics}" &&
                hrm.Headers.Contains("appinsights-mock-header"));
            var httpClient = new Mock<HttpClient>(httpClientHandler);
            httpClientFactory.Setup(hcf => hcf.CreateClient(It.IsAny<string>())).Returns(httpClient.Object);

            var appInsightsHttpClient = new AppInsightsHttpClient(httpClientFactory.Object,
                options.Object,
                requestAuthenticator.Object);

            var result = await appInsightsHttpClient.GetMetricAsync(metrics);

            Assert.Equal(367, result.Aggregation.Sum);

            httpClientFactory.Verify(hcf => hcf.CreateClient(appInsightsConfiguration.ApplicationId), Times.Once);
            requestAuthenticator.Verify(ra => ra.AddAuthenticationAsync(It.IsAny<HttpRequestMessage>(), appInsightsConfiguration), Times.Once);
            requestAuthenticator.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetMetricAsync_Should_ReturnMetric_MultipleConfigurations()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var requestAuthenticator = new Mock<AppInsightsRequestAuthenticator>();
            requestAuthenticator.Setup(ra => ra.AddAuthenticationAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<AppInsightsConfiguration>())).Callback(
                (HttpRequestMessage request, AppInsightsConfiguration configuration) => {
                    request.Headers.Add("appinsights-mock-header", "appinsights-mock-header");
                }
            );

            var appInsightsConfiguration = new AppInsightsConfiguration
            {
                APIKey = "apikey",
                ApplicationId = "appid"
            };

            var secondConfiguration = new AppInsightsConfiguration
            {
                APIKey = "secondapikey",
                ApplicationId = "secondappid"
            };

            var metrics = MetricsDefinition.RequestsCount;

            var httpClientHandler = new HttpClientMockHandler(() => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{'value': {'start': '2019-07-08T18:29:28.972Z','end': '2019-07-09T06:29:28.972Z','requests/count': {'sum': 367}}}")
                }, hrm => hrm.Method == HttpMethod.Get &&
                hrm.RequestUri.ToString() == $"https://api.applicationinsights.io/v1/apps/{appInsightsConfiguration.ApplicationId}/metrics/{metrics}" &&
                hrm.Headers.Contains("appinsights-mock-header"));
            var httpClient = new Mock<HttpClient>(httpClientHandler);
            httpClientFactory.Setup(hcf => hcf.CreateClient(It.IsAny<string>())).Returns(httpClient.Object);

            var appInsightsHttpClient = new AppInsightsHttpClient(httpClientFactory.Object,
                new [] { appInsightsConfiguration, secondConfiguration },
                requestAuthenticator.Object);

            var result = await appInsightsHttpClient.GetMetricAsync(metrics);

            Assert.Equal(367, result.Aggregation.Sum);

            httpClientFactory.Verify(hcf => hcf.CreateClient(appInsightsConfiguration.ApplicationId), Times.Once);
            requestAuthenticator.Verify(ra => ra.AddAuthenticationAsync(It.IsAny<HttpRequestMessage>(), appInsightsConfiguration), Times.Once);
            requestAuthenticator.VerifyNoOtherCalls();
        }
    }
}
