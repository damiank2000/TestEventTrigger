// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace TestEventTrigger;

public class TestEventTrigger3(ILogger<TestEventTrigger3> logger)
{
    private readonly ILogger<TestEventTrigger3> _logger = logger;

    [Function(nameof(TestEventTrigger3))]
    public async Task Run(
        [QueueTrigger("test-queue", Connection = "QueueStorageConnection")] string queueMessage,
        CancellationToken cancellationToken)
    {
        var operationId = GetOperationId();

        _logger.LogInformation("TestEventTrigger: function triggered with operationId {OperationId}.", operationId);

        var rootId = Activity.Current?.RootId;
        var currentId = Activity.Current?.Id;

        _logger.LogInformation("TestEventTrigger: rootId is '{RootId}'", rootId);

        _logger.LogInformation("TestEventTrigger: currentId is '{CurrentId}'", currentId);

        _logger.LogInformation("TestEventTrigger: completed");
    }

    private string GetOperationId()
    {
        var operationId = Activity.Current?.RootId;
        if (operationId is not null)
        {
            _logger.LogInformation("TestEventTrigger: Activity.Current.RootId is {operationId}", operationId);
            return operationId;
        }

        _logger.LogInformation("TestEventTrigger: Activity.Current.RootId is null, checking Activity.Current.Id");
        operationId = Activity.Current?.Id;
        if (operationId is not null)
        {
            _logger.LogInformation("TestEventTrigger: Activity.Current.Id is {operationId}", operationId);
            return operationId;
        }

        _logger.LogInformation("TestEventTrigger: Activity.Current.Id is null, generating new operation ID");
        return Guid.NewGuid().ToString();
    }
}