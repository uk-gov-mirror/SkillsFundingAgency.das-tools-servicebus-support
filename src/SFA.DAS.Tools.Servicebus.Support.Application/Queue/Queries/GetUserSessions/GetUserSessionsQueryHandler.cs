using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.CosmosDb;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetUserSessions;

public class GetUserSessionsQueryHandler(ICosmosUserSessionDbContext cosmosDbContext)
    : IQueryHandler<GetUserSessionsQuery, GetUserSessionsQueryResponse>
{
    public async Task<GetUserSessionsQueryResponse> Handle(GetUserSessionsQuery query)
    {
        var userSessions = await cosmosDbContext.GetUserSessionsAsync();

        return new GetUserSessionsQueryResponse()
        {
            UserSessions = userSessions
        };
    }
}