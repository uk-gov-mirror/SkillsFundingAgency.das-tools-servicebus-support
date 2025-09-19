namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Commands.BulkReplayAllMessages;

public class BulkReplayAllMessagesCommandResponse
{
    public bool Success { get; set; }
    public int TotalMessagesProcessed { get; set; }
    public int TotalBatchesProcessed { get; set; }
    public string ErrorMessage { get; set; }
}