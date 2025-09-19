using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.ServiceBus;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetQueueMessageCount;

public class GetQueueMessageCountQueryHandler(IAsbService asbService)
    : IQueryHandler<GetQueueMessageCountQuery, GetQueueMessageCountQueryResponse>
{
    public async Task<GetQueueMessageCountQueryResponse> Handle(GetQueueMessageCountQuery query)
    {
        var count = await asbService.GetQueueMessageCountAsync(query.QueueName);

        return new GetQueueMessageCountQueryResponse()
        {
            Count = count,
        };
    }
}