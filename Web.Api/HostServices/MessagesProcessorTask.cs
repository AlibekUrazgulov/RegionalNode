using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Web.Api.HostServices;

public class MessagesProcessorTask(MessageProcessorTaskOptions options,
    ILogger<MessagesProcessorTask> logger,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting message processor...");

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Processing new messages...");

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                // var processor = scope.ServiceProvider.GetRequiredService<IMessageProcessor>();
                // await processor.ProcessMessagesAsync(_options.BatchSize, stoppingToken);
            }

            logger.LogInformation("Messages processed, next execution in {OptionsInterval}.", options.Interval);

            await Task.Delay(options.Interval, stoppingToken);
        }
    }
}

public class MessageProcessorTaskOptions
{
    public MessageProcessorTaskOptions(TimeSpan interval, int batchSize)
    {
        Interval = interval;
        BatchSize = batchSize;
    }

    public TimeSpan Interval { get; }
    public int BatchSize { get; }
}
