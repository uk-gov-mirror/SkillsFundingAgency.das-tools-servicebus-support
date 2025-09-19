using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.CosmosDb;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetMessage;

public class GetMessageQueryHandler(ICosmosMessageDbContext cosmosDbContext)
    : IQueryHandler<GetMessageQuery, GetMessageQueryResponse>
{
    public async Task<GetMessageQueryResponse> Handle(GetMessageQuery query)
    {
        var message = await cosmosDbContext.GetQueueMessageAsync(query.UserId, query.MessageId);

        return new GetMessageQueryResponse()
        {
            Message = message,
        };
    }
}