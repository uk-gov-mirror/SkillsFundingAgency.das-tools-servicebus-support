namespace SFA.DAS.Tools.Servicebus.Support.Web.Models;

public class BulkReplayAllMessagesModel
{
    public string QueueName { get; set; }
    public int BatchSize { get; set; } = 100;
}