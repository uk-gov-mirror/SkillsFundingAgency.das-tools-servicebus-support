using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Domain.Queue;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Services;

public interface IMessageService
{
    Task AbortMessages(IEnumerable<QueueMessage> messages, string queue);
    Task ReplayMessages(IEnumerable<QueueMessage> messages, string queue);
}