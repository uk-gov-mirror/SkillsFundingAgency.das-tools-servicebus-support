using System.Collections.Generic;
using SFA.DAS.Tools.Servicebus.Support.Domain.Queue;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetQueueDetails;

public class GetQueueDetailsQueryResponse
{
    public QueueInfo QueueInfo { get; set; }
}