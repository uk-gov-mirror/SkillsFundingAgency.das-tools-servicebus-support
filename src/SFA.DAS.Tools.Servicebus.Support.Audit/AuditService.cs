using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Audit.MessageBuilders;
using SFA.DAS.Tools.Servicebus.Support.Audit.Types;

namespace SFA.DAS.Tools.Servicebus.Support.Audit;

public class AuditService(IAuditApiClient client, IEnumerable<IAuditMessageBuilder> builders)
    : IAuditService
{
    public async Task WriteAudit(AuditMessage message)
    {
        foreach (var builder in builders)
        {
            builder.Build(message);
        }

        await client.Audit(message);
    }
}