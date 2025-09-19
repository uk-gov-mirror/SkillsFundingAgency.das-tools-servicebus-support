using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.ServiceBus;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Commands.SendMessages;

public class SendMessagesCommandHandler(IAsbService asbService)
    : ICommandHandler<SendMessagesCommand, SendMessagesCommandResponse>
{
    public async Task<SendMessagesCommandResponse> Handle(SendMessagesCommand query)
    {
        await asbService.SendMessagesAsync(query.Messages, query.QueueName);

        return new SendMessagesCommandResponse();
    }
}