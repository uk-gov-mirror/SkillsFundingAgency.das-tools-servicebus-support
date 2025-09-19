using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.CosmosDb;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Commands.BulkCreateQueueMessages;

public class BulkCreateQueueMessagesCommandHandler(ICosmosMessageDbContext cosmosDbContext)
    : ICommandHandler<BulkCreateQueueMessagesCommand, BulkCreateQueueMessagesCommandResponse>
{
    public async Task<BulkCreateQueueMessagesCommandResponse> Handle(BulkCreateQueueMessagesCommand query)
    {
        await cosmosDbContext.BulkCreateQueueMessagesAsync(query.Messages);

        return new BulkCreateQueueMessagesCommandResponse();
    }
}