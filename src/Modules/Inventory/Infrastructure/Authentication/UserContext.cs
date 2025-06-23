using Inventory.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Inventory.Infrastructure.Authentication;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long EmployeeId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetPostId() ??
        throw new ApplicationException("Post context is unavailable");

    public long MedicalOrganId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetMedicalOrganId() ??
        throw new ApplicationException("MedicalOrgan context is unavailable");

    public long UserId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new ApplicationException("UserId context is unavailable");

    public string? Token => _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
}
