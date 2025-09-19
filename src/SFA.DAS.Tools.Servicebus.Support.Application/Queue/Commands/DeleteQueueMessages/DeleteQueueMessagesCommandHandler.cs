using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.CosmosDb;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Commands.DeleteQueueMessages;

public class DeleteQueueMessagesCommandHandler(ICosmosMessageDbContext cosmosDbContext)
    : ICommandHandler<DeleteQueueMessagesCommand, DeleteQueueMessagesCommandResponse>
{
    public async Task<DeleteQueueMessagesCommandResponse> Handle(DeleteQueueMessagesCommand query)
    {
        await cosmosDbContext.DeleteQueueMessagesAsync(query.Ids);

        return new DeleteQueueMessagesCommandResponse();
    }
}