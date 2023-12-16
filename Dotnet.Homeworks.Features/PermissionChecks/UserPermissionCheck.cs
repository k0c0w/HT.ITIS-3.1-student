using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Dotnet.Homeworks.Features.PermissionChecks;

public class UserPermissionCheck : PermissionCheck<IClientRequest>
{
    public UserPermissionCheck(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor.HttpContext)
    {
    }

    protected override Task<IEnumerable<PermissionResult>> CheckPermissionFromContextAsync(IClientRequest request, HttpContext httpContext)
    {
        var guid = request.Guid.ToString();
        var requestIdEqualsUserId = httpContext.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier && c.Value == guid);

        return requestIdEqualsUserId ? Task.FromResult(_succedResult) : Task.FromResult(_deniedResult);
    }
}
