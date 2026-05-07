using BankApi.Application.DTOs.Auth;
using BankApi.Application.Interfaces;

namespace BankApi.Application.UseCases.Auth;

public class Login(IAuthService authService, IJwtTokenGenerator jwtGenerator)
{
    public async Task<AuthResponse?> ExecuteAsync(LoginRequest request)
    {
        var result = await authService.LoginAsync(request.Email, request.Password);
        if (result is null) return null;

        var (token, expiresAt) = jwtGenerator.GenerateToken(result.Value.UserId, result.Value.Email);
        return new AuthResponse(token, expiresAt);
    }
}
