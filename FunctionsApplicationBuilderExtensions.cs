using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace TestEventTrigger;

public static class FunctionsApplicationBuilderExtensions
{
    public static FunctionsApplicationBuilder SendLogsDirectlyToAppInsights(this FunctionsApplicationBuilder builder)
    {
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
        //builder.Logging.Services.Configure<LoggerFilterOptions>(options =>
        //{
        //    LoggerFilterRule? defaultRule = options.Rules.FirstOrDefault(rule => rule.ProviderName
        //        == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");
        //    if (defaultRule is not null)
        //    {
        //        options.Rules.Remove(defaultRule);
        //    }
        //});

        return builder;
    }
}
