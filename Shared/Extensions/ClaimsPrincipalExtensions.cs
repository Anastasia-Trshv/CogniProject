using System.Security.Claims;

namespace Cogni.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? user.FindFirst("id")?.Value
                    ?? user.FindFirst("userId")?.Value;

        return int.TryParse(claim, out var id) ? id : null;
    }
}
