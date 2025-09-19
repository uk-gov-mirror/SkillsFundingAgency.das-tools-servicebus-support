using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.CosmosDb;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Commands.UpsertUserSession;

public class UpsertUserSessionCommandHandler(ICosmosUserSessionDbContext cosmosDbContext)
    : ICommandHandler<UpsertUserSessionCommand, UpsertUserSessionCommandResponse>
{
    public async Task<UpsertUserSessionCommandResponse> Handle(UpsertUserSessionCommand query)
    {
        var userSession = await cosmosDbContext.UpsertUserSessionAsync(query.UserSession);

        return new UpsertUserSessionCommandResponse
        {
            UserSession = userSession
        };
    }
}