using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// 1) with none of these lines, I saw logs in App Insights but RootId was null
// but with all of them, I see no traces

// 2) adding this back in meant the logs disappeared from traces
// BUT Root ID was populated in the Test/Run window
// adding this without the other 2) let the function start but couldn't load the Test/Run window
builder.Services.AddApplicationInsightsTelemetryWorkerService();

// 2) what about adding this on its own?
// Function refused to start up!
builder.Services.ConfigureFunctionsApplicationInsights();

// from https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=ihostapplicationbuilder%2Ccode%2Cwindows#application-insights
// disable default log level when writing directly to App Insights
// by default only Warning and above would be logged
builder.Logging.Services.Configure<LoggerFilterOptions>(options =>
{
    LoggerFilterRule? defaultRule = options.Rules.FirstOrDefault(rule => rule.ProviderName
        == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");
    if (defaultRule is not null)
    {
        options.Rules.Remove(defaultRule);
    }
});

// 4) still no traces
//builder.Services.AddLogging();

// 5) putting this back makes no difference
// WHERE ARE ALL THE TRACES NOW?
//builder.Logging.AddApplicationInsights();

// 3) adding this back Test/Run was still working
// but still seeing nothing in traces
//builder.Logging.SetMinimumLevel(LogLevel.Trace);

builder.Build().Run();
