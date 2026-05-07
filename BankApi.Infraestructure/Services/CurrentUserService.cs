using System.Security.Claims;
using BankApi.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BankApi.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string UserId =>
        httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Usuario no autenticado.");
}
