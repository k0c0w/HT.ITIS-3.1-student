using Dotnet.Homeworks.Infrastructure.Utils;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;

public abstract class PermissionCheck<TRequest> : IPermissionCheck<TRequest>
{
    protected static readonly IEnumerable<PermissionResult> _deniedResult = new PermissionResult[] { new PermissionResult(false, "Denied.") };
    protected static readonly IEnumerable<PermissionResult> _succedResult = new[] { new PermissionResult(true) };


    protected readonly HttpContext? _httpContext;

    protected PermissionCheck(HttpContext? httpContext)
    {
        _httpContext = httpContext;
    }

    public Task<IEnumerable<PermissionResult>> CheckPermissionAsync(TRequest request)
    {
        if (request is null || _httpContext is null)
            return Task.FromResult(_deniedResult);

        return CheckPermissionFromContextAsync(request, _httpContext!);
    }

    protected abstract Task<IEnumerable<PermissionResult>> CheckPermissionFromContextAsync(TRequest request, HttpContext httpContext);
}
