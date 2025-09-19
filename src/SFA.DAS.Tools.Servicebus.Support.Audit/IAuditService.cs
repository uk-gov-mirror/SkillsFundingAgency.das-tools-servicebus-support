using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Audit.Types;

namespace SFA.DAS.Tools.Servicebus.Support.Audit;

public interface IAuditService
{
    Task WriteAudit(AuditMessage message);
}