using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.ServiceBus;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.PeekQueueMessages;

public class PeekQueueMessagesQueryHandler(IAsbService asbService)
    : IQueryHandler<PeekQueueMessagesQuery, PeekQueueMessagesQueryResponse>
{
    public async Task<PeekQueueMessagesQueryResponse> Handle(PeekQueueMessagesQuery query)
    {
        var messages = await asbService.PeekMessagesAsync(query.QueueName, query.Quantity);

        return new PeekQueueMessagesQueryResponse()
        {
            Messages = messages
        };
    }
}