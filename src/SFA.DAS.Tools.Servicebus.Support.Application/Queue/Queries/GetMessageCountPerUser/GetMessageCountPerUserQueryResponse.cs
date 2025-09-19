using System.Collections.Generic;
using SFA.DAS.Tools.Servicebus.Support.Domain;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetMessageCountPerUser;

public class GetMessageCountPerUserQueryResponse
{
    public Dictionary<string, List<UserMessageCount>> QueueMessageCount { get; set; }
}