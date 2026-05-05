using System.Security.Claims;
using Devosmos.ApiStarter.Application.Abstractions.Security;
using Microsoft.AspNetCore.Http;

namespace Devosmos.ApiStarter.Infrastructure.Security;

internal sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal? Principal => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;

    public string? UserId => Principal?.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? Principal?.FindFirstValue("sub")
        ?? Principal?.FindFirstValue("oid");

    public string? UserName => Principal?.Identity?.Name
        ?? Principal?.FindFirstValue("preferred_username")
        ?? Principal?.FindFirstValue("name");
}
