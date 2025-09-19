using System.Collections.Generic;
using SFA.DAS.Tools.Servicebus.Support.Domain;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetExpiredUserSessions;

public class GetExpiredUserSessionsQueryResponse
{
    public IEnumerable<UserSession> ExpiredUserSessions { get; set; }
}