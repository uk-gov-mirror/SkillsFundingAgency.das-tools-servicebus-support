using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using SFA.DAS.Tools.Servicebus.Support.Application.Services;
using SFA.DAS.Tools.Servicebus.Support.Domain.Configuration;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure;

namespace SFA.DAS.Tools.Servicebus.Support.Web.App_Start;

public class KeepUserSessionActiveFilter(
    IUserSessionService userSessionService,
    UserIdentitySettings userIdentitySettings)
    : IAsyncActionFilter
{
    private readonly int _userSessionRefreshIntervalMinutes = userIdentitySettings.UserRefreshSessionIntervalMinutes;


    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var sessionActiveUntil = context.HttpContext.Session.Get<DateTime?>("sessionActiveUntil");
        if (!sessionActiveUntil.HasValue || sessionActiveUntil < DateTime.UtcNow)
        {
            var userSession = await userSessionService.GetUserSession();
            if(userSession != null)
            {
                // Update Session Expiry time once within the sessionActive window
                await userSessionService.UpsertUserSession(userSession.Queue);

                // Set sessionActive window to prevent constant updating
                var sessionExpiry = DateTime.UtcNow.AddMinutes(_userSessionRefreshIntervalMinutes);
                context.HttpContext.Session.Set("sessionActiveUntil", sessionExpiry);
            }

        }

        await next();
    }
}