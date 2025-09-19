using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Domain;

namespace SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.CosmosDb;

public interface ICosmosUserSessionDbContext
{
    Task<UserSession> UpsertUserSessionAsync(UserSession userSession);
    Task<UserSession> GetUserSessionAsync(string userId);
    Task DeleteUserSessionAsync(string id, string userId);
    Task<IEnumerable<UserSession>> GetExpiredUserSessionsAsync();
    Task<IEnumerable<UserSession>> GetUserSessionsAsync();
}