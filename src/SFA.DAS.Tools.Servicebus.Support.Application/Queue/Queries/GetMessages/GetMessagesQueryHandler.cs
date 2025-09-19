using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.CosmosDb;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetMessages;

public class GetMessagesQueryHandler(ICosmosMessageDbContext cosmosDbContext)
    : IQueryHandler<GetMessagesQuery, GetMessagesQueryResponse>
{
    public async Task<GetMessagesQueryResponse> Handle(GetMessagesQuery query)
    {
        var messages = cosmosDbContext.GetQueueMessagesAsync(query.UserId, query.SearchProperties);
        var unfilteredCnt = cosmosDbContext.GetMessageCountAsync(query.UserId);
        var cnt = cosmosDbContext.GetMessageCountAsync(query.UserId, new SearchProperties()
        {
            Search = query.SearchProperties.Search
        });

        await Task.WhenAll(messages, cnt, unfilteredCnt);

        return new GetMessagesQueryResponse()
        {
            Messages = await messages,
            Count = await cnt,
            UnfilteredCount = await unfilteredCnt
        };
    }
}