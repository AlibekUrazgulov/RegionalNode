using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Inventory.Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static long GetMedicalOrganId(this ClaimsPrincipal? principal)
    {
        string? currentPostClaim = principal?.FindFirstValue("Damumed.CurrentPost");

        if (string.IsNullOrEmpty(currentPostClaim))
        {
            throw new ApplicationException("Claim 'Damumed.CurrentPost' is unavailable");
        }

        using var json = System.Text.Json.JsonDocument.Parse(currentPostClaim);
        return json.RootElement.GetProperty("OrgHealthCareID").GetInt64();
    }

    public static long GetPostId(this ClaimsPrincipal? principal)
    {
        string? currentPostClaim = principal?.FindFirstValue("Damumed.CurrentPost");

        if (string.IsNullOrEmpty(currentPostClaim))
        {
            throw new ApplicationException("Claim 'Damumed.CurrentPost' is unavailable");
        }

        using var json = System.Text.Json.JsonDocument.Parse(currentPostClaim);
        return json.RootElement.GetProperty("PostID").GetInt64();
    }

    public static long GetUserId(this ClaimsPrincipal? principal)
    {
        string userId = principal?.FindFirstValue("Damumed.UserID") ?? throw new ApplicationException("Claim 'Damumed.UserID' is unavailable");

        return long.Parse(userId, CultureInfo.InvariantCulture);
    }
}
