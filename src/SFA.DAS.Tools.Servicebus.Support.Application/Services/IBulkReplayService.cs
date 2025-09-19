using System.Threading.Tasks;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Services;

public interface IBulkReplayService
{
    Task<string> StartBulkReplayAsync(string userId, string queueName, int batchSize = 100);
    Task<BulkReplayStatus> GetBulkReplayStatusAsync(string operationId);
}

public class BulkReplayStatus
{
    public string OperationId { get; set; }
    public string Status { get; set; } // "Running", "Completed", "Failed"
    public int TotalMessagesProcessed { get; set; }
    public int TotalBatchesProcessed { get; set; }
    public string ErrorMessage { get; set; }
    public bool IsComplete => Status == "Completed" || Status == "Failed";
}