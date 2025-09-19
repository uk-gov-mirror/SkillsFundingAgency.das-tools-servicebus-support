using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.ServiceBus;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetQueueDetails;

public class GetQueueDetailsQueryHandler(IAsbService asbService)
    : IQueryHandler<GetQueueDetailsQuery, GetQueueDetailsQueryResponse>
{
    public async Task<GetQueueDetailsQueryResponse> Handle(GetQueueDetailsQuery query)
    {
        var queueInfo = await asbService.GetQueueDetailsAsync(query.QueueName);

        return new GetQueueDetailsQueryResponse()
        {
            QueueInfo = queueInfo
        };
    }
}