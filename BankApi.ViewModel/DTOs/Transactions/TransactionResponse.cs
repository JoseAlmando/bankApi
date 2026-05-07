namespace BankApi.Application.DTOs.Transactions;

public record TransactionResponse(
    Guid Id,
    Guid AccountId,
    decimal Amount,
    string Type,
    DateTime CreatedAt);
