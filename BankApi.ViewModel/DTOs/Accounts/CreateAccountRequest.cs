namespace BankApi.Application.DTOs.Accounts;

public record CreateAccountRequest(decimal InitialBalance = 0);
