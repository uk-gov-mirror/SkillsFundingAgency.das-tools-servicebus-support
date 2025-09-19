using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.CosmosDb;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetExpiredUserSessions;

public class GetExpiredUserSessionsQueryHandler(ICosmosUserSessionDbContext cosmosDbContext)
    : IQueryHandler<GetExpiredUserSessionsQuery, GetExpiredUserSessionsQueryResponse>
{
    public async Task<GetExpiredUserSessionsQueryResponse> Handle(GetExpiredUserSessionsQuery query)
    {
        var expiredUserSessions = await cosmosDbContext.GetExpiredUserSessionsAsync();

        return new GetExpiredUserSessionsQueryResponse
        {
            ExpiredUserSessions = expiredUserSessions
        };
    }
}