using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetMessages;
using SFA.DAS.Tools.Servicebus.Support.Application.Services;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Commands.BulkReplayAllMessages;

public class BulkReplayAllMessagesCommandHandler(
    IQueryHandler<GetMessagesQuery, GetMessagesQueryResponse> getMessagesQuery,
    IMessageService messageService,
    ILogger<BulkReplayAllMessagesCommandHandler> logger)
    : ICommandHandler<BulkReplayAllMessagesCommand, BulkReplayAllMessagesCommandResponse>
{
    public async Task<BulkReplayAllMessagesCommandResponse> Handle(BulkReplayAllMessagesCommand command)
    {
        try
        {
            logger.LogInformation("Starting bulk replay of all messages for user {UserId} in queue {QueueName}", 
                command.UserId, command.QueueName);

            var totalMessagesProcessed = 0;
            var totalBatchesProcessed = 0;
            var offset = 0;
            var hasMoreMessages = true;

            while (hasMoreMessages)
            {
                var response = await getMessagesQuery.Handle(new GetMessagesQuery
                {
                    UserId = command.UserId,
                    SearchProperties = new SearchProperties
                    {
                        Offset = offset,
                        Limit = command.BatchSize
                    }
                });

                var messages = response.Messages.ToList();
                    
                if (!messages.Any())
                {
                    hasMoreMessages = false;
                    break;
                }

                await messageService.ReplayMessages(messages, command.QueueName);
                    
                totalMessagesProcessed += messages.Count;
                totalBatchesProcessed++;
                offset += command.BatchSize;

                logger.LogInformation("Processed batch {BatchNumber} with {MessageCount} messages. Total processed: {TotalMessages}", 
                    totalBatchesProcessed, messages.Count, totalMessagesProcessed);

                if (messages.Count < command.BatchSize)
                {
                    hasMoreMessages = false;
                }
            }

            logger.LogInformation("Completed bulk replay. Total messages processed: {TotalMessages}, Total batches: {TotalBatches}", 
                totalMessagesProcessed, totalBatchesProcessed);

            return new BulkReplayAllMessagesCommandResponse
            {
                Success = true,
                TotalMessagesProcessed = totalMessagesProcessed,
                TotalBatchesProcessed = totalBatchesProcessed
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to bulk replay messages for user {UserId} in queue {QueueName}", 
                command.UserId, command.QueueName);

            return new BulkReplayAllMessagesCommandResponse
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }
}