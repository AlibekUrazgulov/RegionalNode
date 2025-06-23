using Inventory.Application.Abstractions.Data;
using Inventory.Application.OutboxMessages;
using Microsoft.Data.SqlClient;
using Dapper;
using Inventory.SharedKernel;

namespace Inventory.Infrastructure.OutboxMessages;

public class OutboxMessageQueryQueryRepository(IConnectionStringProvider connectionStringProvider,
    IDateTimeProvider dateTimeProvider) : IOutboxMessageQueryRepository
{
    private const string query = @"UPDATE TOP (@BatchSize) OutboxMessageTasks
                                        SET PlannedProcessDate = @DisplacementDate
                                        OUTPUT Inserted.Id, Inserted.MessageType, Inserted.Content
                                    WHERE PlannedProcessDate > @NowDate
                                        AND MessageType IN @MessageTypes
                                    ORDER BY ID
                                ";

    public async Task<List<OutboxMessageTaskOutput>> FetchUnprocessedAsync(OutboxMessageType messageType, int batchSize, int displacementMinute,
        CancellationToken cancellationToken)
    {
        if (batchSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(batchSize), @"Batch size must be greater than zero.");
        }
        if (displacementMinute <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(displacementMinute), @"Displacement minute must be greater than zero.");
        }

        await using var conn = new SqlConnection(connectionStringProvider.ConnectionString);
        IEnumerable<OutboxMessageTaskOutput> result = await conn.QueryAsync<OutboxMessageTaskOutput>(query, new
        {
            BatchSize = batchSize,
            NowDate = dateTimeProvider.ServerNow,
            DisplacementDate = dateTimeProvider.ServerNow.AddMinutes(displacementMinute),
            MessageTypes = new[] { messageType }
        });
        return result.ToList();
    }

    public async Task<List<OutboxMessageTaskOutput>> FetchUnprocessedAsync(List<OutboxMessageType> messageTypes, int batchSize, int displacementMinute,
        CancellationToken cancellationToken)
    {
        if (batchSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(batchSize), @"Batch size must be greater than zero.");
        }
        if (displacementMinute <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(displacementMinute), @"Displacement minute must be greater than zero.");
        }

        await using var conn = new SqlConnection(connectionStringProvider.ConnectionString);
        IEnumerable<OutboxMessageTaskOutput> result = await conn.QueryAsync<OutboxMessageTaskOutput>(query, new
        {
            BatchSize = batchSize,
            NowDate = dateTimeProvider.ServerNow,
            DisplacementDate = dateTimeProvider.ServerNow.AddMinutes(displacementMinute),
            MessageTypes = messageTypes
        });
        return result.ToList();
    }
}
