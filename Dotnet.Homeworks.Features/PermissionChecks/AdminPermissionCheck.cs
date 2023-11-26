using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.Enums;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.PermissionChecks;

internal class AdminPermissionCheck : PermissionCheck<IAdminRequest>
{
    private static readonly string _adminRoleString = Roles.Admin.ToString();

    public AdminPermissionCheck(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor.HttpContext)
    {
    }

    protected override Task<IEnumerable<PermissionResult>> CheckPermissionFromContextAsync(IAdminRequest request, HttpContext httpContext)
    {
        var hasAdminRole = httpContext.User.IsInRole(_adminRoleString);

        return hasAdminRole ? TaskFromResult(_succedResult) : TaskFromResult(_deniedResult);
    }

    private static Task<IEnumerable<PermissionResult>> TaskFromResult(IEnumerable<PermissionResult> result) => Task.FromResult(result);
}
