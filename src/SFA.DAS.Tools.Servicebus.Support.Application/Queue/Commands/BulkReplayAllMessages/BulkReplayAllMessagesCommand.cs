using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Commands.BulkReplayAllMessages;

public class BulkReplayAllMessagesCommand
{
    public string UserId { get; set; }
    public string QueueName { get; set; }
    public int BatchSize { get; set; } = 100;
}