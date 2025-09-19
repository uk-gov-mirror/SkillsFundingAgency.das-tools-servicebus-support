using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.CosmosDb;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Queue.Commands.DeleteUserSession;

public class DeleteUserSessionCommandHandler(ICosmosUserSessionDbContext cosmosDbContext)
    : ICommandHandler<DeleteUserSessionCommand, DeleteUserSessionCommandResponse>
{
    public async Task<DeleteUserSessionCommandResponse> Handle(DeleteUserSessionCommand query)
    {
        await cosmosDbContext.DeleteUserSessionAsync(query.Id, query.UserId);

        return new DeleteUserSessionCommandResponse();
    }
}