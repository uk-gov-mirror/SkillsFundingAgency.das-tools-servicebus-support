using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Commands.DeleteUserSession;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Commands.UpsertUserSession;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetUserSession;
using SFA.DAS.Tools.Servicebus.Support.Domain;
using SFA.DAS.Tools.Servicebus.Support.Domain.Configuration;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services;

namespace SFA.DAS.Tools.Servicebus.Support.Application.Services;

public class UserSessionService(
    ICommandHandler<UpsertUserSessionCommand, UpsertUserSessionCommandResponse> upsertUserSessionCommand,
    IQueryHandler<GetUserSessionQuery, GetUserSessionQueryResponse> getUserSessionQuery,
    ICommandHandler<DeleteUserSessionCommand, DeleteUserSessionCommandResponse> deleteUserSessionCommand,
    IUserService userService,
    UserIdentitySettings config,
    IHttpContextAccessor httpContextAccessor)
    : IUserSessionService
{
    private readonly int _userSessionExpiryHours = config.UserSessionExpiryHours <= 0 ? 24 : config.UserSessionExpiryHours;

    public async Task<UserSession> UpsertUserSession(string queue)
    {
        var userSession = await GetUserSession();

        if (userSession != null)
        {
            // Update Expiry
            userSession.ExpiryDateUtc = DateTime.UtcNow.AddHours(_userSessionExpiryHours);
        }
        else
        {
            // Create new session
            userSession = CreateUserSession(queue);
        }

        var result = await upsertUserSessionCommand.Handle(new UpsertUserSessionCommand()
        {
            UserSession = userSession
        });

        return result.UserSession;
    }

    public async Task DeleteUserSession()
    {
        var currentSession = httpContextAccessor.HttpContext.Session.Get<UserSession>("userSession");
        var userSession = currentSession ?? await GetUserSession();

        if (userSession != null)
        {
            await deleteUserSessionCommand.Handle(new DeleteUserSessionCommand()
            {
                Id = userSession.Id,
                UserId = userService.GetUserId()
            });
        }
    }

    public async Task<UserSession> GetUserSession()
    {
        var result = await getUserSessionQuery.Handle(new GetUserSessionQuery()
        {
            UserId = userService.GetUserId()
        });

        return result.UserSession;
    }

    private UserSession CreateUserSession(string queueName)
    {
        return new UserSession
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userService.GetUserId(),
            UserName = userService.GetName(),
            ExpiryDateUtc = DateTime.UtcNow.AddHours(_userSessionExpiryHours),
            Queue = queueName
        };
    }

}