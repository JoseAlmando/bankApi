namespace BankApi.Application.DTOs.Auth;

public record AuthResponse(string Token, DateTime ExpiresAt);
