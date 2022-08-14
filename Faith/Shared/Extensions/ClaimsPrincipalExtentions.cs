using System.Security.Claims;

namespace Faith.Shared.Extensions;

public static class ClaimsPrincipalExtentions
{
    public static string? GetClaimValue(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        return claimsPrincipal.
            Claims.
            FirstOrDefault(c => c.Type == claimType)?.Value;
    }
}