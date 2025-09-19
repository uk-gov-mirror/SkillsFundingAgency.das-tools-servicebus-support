using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.ServiceBus;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.ReceiveQueueMessages;

public class ReceiveQueueMessagesQueryHandler(IAsbService asbService)
    : IQueryHandler<ReceiveQueueMessagesQuery, ReceiveQueueMessagesQueryResponse>
{
    public async Task<ReceiveQueueMessagesQueryResponse> Handle(ReceiveQueueMessagesQuery query)
    {
        var messages = await asbService.ReceiveMessagesAsync(query.QueueName, query.Quantity);

        return new ReceiveQueueMessagesQueryResponse()
        {
            Messages = messages
        };
    }
}