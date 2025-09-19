using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SFA.DAS.Tools.Servicebus.Support.Application;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Commands.DeleteUserSession;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetExpiredUserSessions;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetMessages;
using SFA.DAS.Tools.Servicebus.Support.Application.Services;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services;

namespace SFA.DAS.Tools.Servicebus.Support.Functions;

public class CleanExpiredUserSessionsFunction(
    IQueryHandler<GetExpiredUserSessionsQuery, GetExpiredUserSessionsQueryResponse> expiredUserSessionQuery,
    IQueryHandler<GetMessagesQuery, GetMessagesQueryResponse> getMessagesQuery,
    IMessageService messageService,
    ICommandHandler<DeleteUserSessionCommand, DeleteUserSessionCommandResponse> deleteUserSessionCommand,
    IUserService userService,
    ILogger<CleanExpiredUserSessionsFunction> logger)
{
    [Function("CleanExpiredUserSessionsFunction")]
    public async Task Run([TimerTrigger("%CleanExpiredUserSessionsFunctionTimer%", RunOnStartup = false)] TimerInfo myTimer)
    {
        try
        {
            var queryResult = await expiredUserSessionQuery.Handle(new GetExpiredUserSessionsQuery());

            foreach (var session in queryResult.ExpiredUserSessions)
            {
                userService.Configure(session.UserId, "CleanExpiredUserSessionsFunction");

                var getMessagesResponse = await GetMessages(session.UserId);

                while (getMessagesResponse.Messages.Any())
                {
                    await messageService.AbortMessages(getMessagesResponse.Messages, session.Queue);
                    getMessagesResponse = await GetMessages(session.UserId);
                }

                if (!getMessagesResponse.Messages.Any())
                {
                    await deleteUserSessionCommand.Handle(new DeleteUserSessionCommand
                    {
                        Id = session.Id,
                        UserId = session.UserId
                    });
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CleanExpiredUserSessionsFunction failed");
        }
    }

    private async Task<GetMessagesQueryResponse> GetMessages(string userId)
    {
        return await getMessagesQuery.Handle(new GetMessagesQuery
        {
            UserId = userId,
            SearchProperties = new SearchProperties
            {
                Offset = 0,
                Limit = 100
            }
        });
    }
}