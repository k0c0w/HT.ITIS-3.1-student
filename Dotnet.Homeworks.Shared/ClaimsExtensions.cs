using System.Security.Claims;

namespace Dotnet.Homeworks.Shared.Extensions;

public static class ClaimsExtensions
{

    public static Guid? GetUserId(this ClaimsPrincipal claims)
    {
        if (Guid.TryParse(claims.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            return userId;

        return default;
    }
}
