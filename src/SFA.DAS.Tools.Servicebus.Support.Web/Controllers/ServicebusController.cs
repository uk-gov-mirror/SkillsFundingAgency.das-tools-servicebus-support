using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Tools.Servicebus.Support.Application;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Commands.BulkCreateQueueMessages;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Commands.SendMessages;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetMessages;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetUserSession;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.PeekQueueMessages;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services;
using SFA.DAS.Tools.Servicebus.Support.Web.Models;

namespace SFA.DAS.Tools.Servicebus.Support.Web.Controllers;

public class ServicebusController(
    ILogger<ServicebusController> logger,
    IUserService userService,
    IQueryHandler<GetUserSessionQuery, GetUserSessionQueryResponse> getUserSessionQuery,
    IQueryHandler<PeekQueueMessagesQuery, PeekQueueMessagesQueryResponse> peekQueueMessagesQuery,
    ICommandHandler<BulkCreateQueueMessagesCommand, BulkCreateQueueMessagesCommandResponse> bulkCreateMessagesCommand,
    ICommandHandler<SendMessagesCommand, SendMessagesCommandResponse> sendMessagesCommand,
    IQueryHandler<GetMessagesQuery, GetMessagesQueryResponse> getMessagesQuery)
    : Controller
{
    private readonly ILogger<ServicebusController> _logger = logger;

    public async Task<IActionResult> Index()
    {
        var response = await getUserSessionQuery.Handle(new GetUserSessionQuery()
        {
            UserId = userService.GetUserId()
        });

        if (response.HasSession())
        {
            return RedirectToAction(actionName: "Index", controllerName: "MessageList");
        }            

        return View();
    }              

#if DEBUG
    public async Task<IActionResult> ImportToCosmos(string queueName = null)
    {
        queueName ??= "sfa.das.notifications.messagehandlers-errors";

        var response = await peekQueueMessagesQuery.Handle(new PeekQueueMessagesQuery()
        {
            QueueName = queueName
        });

        await bulkCreateMessagesCommand.Handle(new BulkCreateQueueMessagesCommand()
        {
            Messages = response.Messages
        });

        return RedirectToAction(actionName: "Index", controllerName: "Home");
    }

    public async Task<IActionResult> ImportToQueue(string queueName)
    {
        var response = await getMessagesQuery.Handle(new GetMessagesQuery()
        {
            UserId = "123456",
            SearchProperties = new SearchProperties()
        });

        await sendMessagesCommand.Handle(new SendMessagesCommand()
        {
            Messages = response.Messages,
            QueueName = queueName
        });

        return RedirectToAction(actionName: "Index", controllerName: "Home");
    }
#endif

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}