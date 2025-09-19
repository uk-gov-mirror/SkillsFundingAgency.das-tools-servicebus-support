using System.Threading.Tasks;
using SFA.DAS.Tools.Servicebus.Support.Domain;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Services;

public interface IUserSessionService
{
    Task<UserSession> UpsertUserSession(string queue);
    Task<UserSession> GetUserSession();
    Task DeleteUserSession();
}