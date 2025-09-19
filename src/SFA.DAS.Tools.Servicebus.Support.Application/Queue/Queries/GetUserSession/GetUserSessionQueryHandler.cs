using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.CosmosDb;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetUserSession;

public class GetUserSessionQueryHandler(ICosmosUserSessionDbContext cosmosDbContext)
    : IQueryHandler<GetUserSessionQuery, GetUserSessionQueryResponse>
{
    public async Task<GetUserSessionQueryResponse> Handle(GetUserSessionQuery query)
    {
        var userSession = await cosmosDbContext.GetUserSessionAsync(query.UserId);

        return new GetUserSessionQueryResponse()
        {
            UserSession = userSession
        };
    }
}