using System.Linq;
using Microsoft.AspNetCore.Http;
using SFA.DAS.Tools.Servicebus.Support.Domain.Configuration;

namespace SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services;

public class UserService(IHttpContextAccessor httpContextAccessor, UserIdentitySettings userIdentitySettings)
    : IUserService
{
    private readonly string _nameClame = userIdentitySettings.NameClaim;

    public string GetUserId()
    {
        return httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type.Contains("nameidentifier")).Select(x => x.Value).FirstOrDefault();
    }

    public string GetName()
    {
        return httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == _nameClame).Select(x => x.Value).FirstOrDefault();
    }

    public void Configure(string userId, string userName)
    {
        throw new System.NotImplementedException();
    }
}