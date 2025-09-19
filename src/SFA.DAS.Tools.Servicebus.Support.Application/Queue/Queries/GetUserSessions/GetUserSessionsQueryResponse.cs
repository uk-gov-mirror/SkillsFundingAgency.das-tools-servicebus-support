using System.Collections.Generic;
using SFA.DAS.Tools.Servicebus.Support.Domain;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetUserSessions;

public class GetUserSessionsQueryResponse
{
    public IEnumerable<UserSession> UserSessions { get; set; }
}