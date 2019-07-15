[![Build status](https://aleclerc.visualstudio.com/azure-appinsights-api-wrapper/_apis/build/status/AppInsights.Http%20-%20Nuget)](https://aleclerc.visualstudio.com/azure-appinsights-api-wrapper/_build/latest?definitionId=6) [![NuGet](https://img.shields.io/nuget/v/AppInsights.Http.svg)](https://www.nuget.org/packages/AppInsights.Http/)

# AppInsights.Http

This is a prototype of a library allowing to retrieve data from the Application Insights API.

## Authentication to the API

So far, only the API key authentication is supported. 

## Using this library

An extension method `AddAppInsightsHttpClient` allows you to register the services of this library on the service collection container. You can pass the configuration to use to authenticate to the Application Insights API while calling this method.

You can then inject an instance of `IAppInsightsHttpClient`. This class allows you to retrieve metrics based on a defined type, fetch the metadata or execute queries on the analytics level.