using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using TestEventTrigger;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// 1) with none of these lines, I saw logs in App Insights but RootId was null
// but with all of them, I see no traces

// With this call in the logs don't appear in traces
// BUT Root ID was populated in the Test/Run window
builder.SendLogsDirectlyToAppInsights();

// 4) still no traces
//builder.Services.AddLogging();

// 5) putting this back makes no difference
// WHERE ARE ALL THE TRACES NOW?
//builder.Logging.AddApplicationInsights();

// 3) adding this back Test/Run was still working
// but still seeing nothing in traces
//builder.Logging.SetMinimumLevel(LogLevel.Trace);

builder.Build().Run();
