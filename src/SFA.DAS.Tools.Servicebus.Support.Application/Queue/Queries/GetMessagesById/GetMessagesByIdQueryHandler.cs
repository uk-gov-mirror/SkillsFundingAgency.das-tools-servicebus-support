using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.CosmosDb;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetMessagesById;

public class GetMessagesByIdQueryHandler(ICosmosMessageDbContext cosmosDbContext)
    : IQueryHandler<GetMessagesByIdQuery, GetMessagesByIdQueryResponse>
{
    public async Task<GetMessagesByIdQueryResponse> Handle(GetMessagesByIdQuery query)
    {
        var messages = await cosmosDbContext.GetQueueMessagesByIdAsync(query.UserId, query.Ids);

        return new GetMessagesByIdQueryResponse()
        {
            Messages = messages
        };
    }
}