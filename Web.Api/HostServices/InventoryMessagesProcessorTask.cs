using Inventory.Application.OutboxMessages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Web.Api.HostServices;

public class InventoryMessagesProcessorTask(MessageProcessorTaskOptions options,
    IOutboxMessageProcessorFacade iOutboxMessageProcessorFacade,
    ILogger<InventoryMessagesProcessorTask> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting message processor...");

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Processing new messages...");

            await iOutboxMessageProcessorFacade.ProcessMessagesAsync(options.BatchSize, options.DisplacementMinute,
                stoppingToken);

            logger.LogInformation("Messages processed, next execution in {OptionsInterval}.", options.Interval);

            await Task.Delay(options.Interval, stoppingToken);
        }
    }
}

public class MessageProcessorTaskOptions
{
    public MessageProcessorTaskOptions(TimeSpan interval, int batchSize, int displacementMinute = 0)
    {
        if (interval <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(interval), @"Interval must be greater than zero.");
        }

        if (batchSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(batchSize), @"Batch size must be greater than zero.");
        }

        DisplacementMinute = displacementMinute;
        Interval = interval;
        BatchSize = batchSize;
    }

    public TimeSpan Interval { get; }
    public int BatchSize { get; }
    public int DisplacementMinute { get; }
}
