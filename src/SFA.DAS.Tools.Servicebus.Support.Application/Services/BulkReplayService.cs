using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Commands.BulkReplayAllMessages;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Services;

public class BulkReplayService(
    ICommandHandler<BulkReplayAllMessagesCommand, BulkReplayAllMessagesCommandResponse> bulkReplayCommand,
    ILogger<BulkReplayService> logger)
    : IBulkReplayService
{
    private readonly ConcurrentDictionary<string, BulkReplayStatus> _operationStatuses = new();

    public async Task<string> StartBulkReplayAsync(string userId, string queueName, int batchSize = 100)
    {
        var operationId = Guid.NewGuid().ToString();
            
        var status = new BulkReplayStatus
        {
            OperationId = operationId,
            Status = "Running",
            TotalMessagesProcessed = 0,
            TotalBatchesProcessed = 0
        };

        _operationStatuses[operationId] = status;

        // Start the bulk replay operation in the background
        _ = Task.Run(async () =>
        {
            try
            {
                logger.LogInformation("Starting background bulk replay operation {OperationId} for user {UserId} in queue {QueueName}", 
                    operationId, userId, queueName);

                var command = new BulkReplayAllMessagesCommand
                {
                    UserId = userId,
                    QueueName = queueName,
                    BatchSize = batchSize
                };

                var result = await bulkReplayCommand.Handle(command);

                if (result.Success)
                {
                    status.Status = "Completed";
                    status.TotalMessagesProcessed = result.TotalMessagesProcessed;
                    status.TotalBatchesProcessed = result.TotalBatchesProcessed;
                        
                    logger.LogInformation("Completed background bulk replay operation {OperationId}. Processed {TotalMessages} messages in {TotalBatches} batches", 
                        operationId, result.TotalMessagesProcessed, result.TotalBatchesProcessed);
                }
                else
                {
                    status.Status = "Failed";
                    status.ErrorMessage = result.ErrorMessage;
                        
                    logger.LogError("Failed background bulk replay operation {OperationId}. Error: {ErrorMessage}", 
                        operationId, result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                status.Status = "Failed";
                status.ErrorMessage = ex.Message;
                    
                logger.LogError(ex, "Exception in background bulk replay operation {OperationId}", operationId);
            }
        });

        return operationId;
    }

    public Task<BulkReplayStatus> GetBulkReplayStatusAsync(string operationId)
    {
        if (_operationStatuses.TryGetValue(operationId, out var status))
        {
            return Task.FromResult(status);
        }

        return Task.FromResult(new BulkReplayStatus
        {
            OperationId = operationId,
            Status = "NotFound",
            ErrorMessage = "Operation not found"
        });
    }
}