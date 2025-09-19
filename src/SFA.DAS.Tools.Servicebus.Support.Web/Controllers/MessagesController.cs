using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Tools.Servicebus.Support.Application;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetMessages;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services;

namespace SFA.DAS.Tools.Servicebus.Support.Web.Controllers;

public class MessagesController(
    IUserService userService,
    IQueryHandler<GetMessagesQuery, GetMessagesQueryResponse> getMessagesQuery)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string sort, string order, string search, int offset, int limit)
    {
        var response = await getMessagesQuery.Handle(new GetMessagesQuery()
        {
            UserId = userService.GetUserId(),
            SearchProperties = new SearchProperties
            {
                Sort = sort,
                Order = order,
                Search = search,
                Offset = offset,
                Limit = limit
            }
        });

        var queueMessages = response.Messages.ToList();

        return Json(new
        {
            Total = response.Count,
            TotalNotFiltered = response.UnfilteredCount,
            Rows = queueMessages.Select(msg => new
            {
                Id = msg.Id,
                OriginatingEndpoint = msg.OriginatingEndpoint,
                ProcessingEndpoint = msg.ProcessingEndpoint,
                Body = msg.Body,
                Exception = msg.Exception,
                ExceptionType = msg.ExceptionType
            })
        });
    }
}