using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestEventTrigger;

public static class FunctionsApplicationBuilderExtensions
{
    public static FunctionsApplicationBuilder RouteLoggingDirectlyToAppInsights(this FunctionsApplicationBuilder builder)
    {
        // Route logging directly to App Insights, rather than relaying logs through the host.
        // This means that Activity.Current.RootId is populated with the operation_Id,
        // allowing it to be used in code.
        // https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=ihostapplicationbuilder%2Ccode%2Cwindows#application-insights
        builder.Services
            .AddApplicationInsightsTelemetryWorkerService()
            .ConfigureFunctionsApplicationInsights();

        // Disable default log level rule when writing directly to App Insights.
        // By default only Warning and above would be logged.
        // https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=ihostapplicationbuilder%2Ccode%2Cwindows#managing-log-levels
        builder.Logging.Services.Configure<LoggerFilterOptions>(options =>
        {
            LoggerFilterRule? defaultRule = options.Rules.FirstOrDefault(rule => rule.ProviderName
                == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");
            if (defaultRule is not null)
            {
                options.Rules.Remove(defaultRule);
            }
        });

        return builder;
    }
}
