using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestEventTrigger;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// 1) with none of these lines, I saw logs in App Insights but RootId was null
// but with all of them, I see no traces

// With this call in the logs don't appear in traces
// BUT Root ID was populated in the Test/Run window
builder.RouteLoggingDirectlyToAppInsights();

// "Adds the logging framework"
// Probably useless
builder.Services.AddLogging();

// "Send ILogger logs to Application Insights"
// Should happen anyway by default
builder.Logging.AddApplicationInsights();

// "Don't even consider logs below this level"
// With the default Worker logging, this is controlled by host.json
// and with logging direct to App Insights, you have to remove the rule via code.
builder.Logging.SetMinimumLevel(LogLevel.Trace);

builder.Build().Run();
