using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Audit.Types;

namespace SFA.DAS.Tools.Servicebus.Support.Audit;

public interface IAuditApiClient
{
    Task Audit(AuditMessage message);
}