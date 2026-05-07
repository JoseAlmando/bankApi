namespace BankApi.Application.DTOs.Accounts;

public record AccountResponse(
    Guid Id,
    string AccountNumber,
    decimal Balance,
    DateTime CreatedAt);
