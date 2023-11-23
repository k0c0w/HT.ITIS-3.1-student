using Dotnet.Homeworks.Infrastructure.Utils;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;

public interface IPermissionCheck<TRequest>
{
    Task<IEnumerable<PermissionResult>> CheckPermissionAsync(TRequest request);
}