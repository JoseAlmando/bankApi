using BankApi.Application.DTOs.Auth;
using BankApi.Application.Interfaces;

namespace BankApi.Application.UseCases.Auth;

public class Register(IAuthService authService, IJwtTokenGenerator jwtGenerator)
{
    public async Task<AuthResponse> ExecuteAsync(RegisterRequest request)
    {
        var (userId, email) = await authService.RegisterAsync(
            request.FirstName, request.LastName, request.Email, request.Password);

        var (token, expiresAt) = jwtGenerator.GenerateToken(userId, email);
        return new AuthResponse(token, expiresAt);
    }
}
