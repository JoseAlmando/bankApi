using BankApi.Application.DTOs.Transactions;
using BankApi.Application.Interfaces;

namespace BankApi.Application.UseCases.Transactions;

public class GetTransactionsByAccount(
    IAccountRepository accounts,
    ITransactionRepository transactions,
    ICurrentUserService currentUser)
{
    public async Task<IReadOnlyList<TransactionResponse>> ExecuteAsync(Guid accountId, CancellationToken ct = default)
    {
        var account = await accounts.GetByIdAsync(accountId, ct)
            ?? throw new KeyNotFoundException($"Cuenta {accountId} no encontrada.");

        if (account.OwnerId != currentUser.UserId)
            throw new UnauthorizedAccessException("No tienes acceso a esta cuenta.");

        var list = await transactions.ListByAccountIdAsync(accountId, ct);
        return list
            .Select(t => new TransactionResponse(t.Id, t.AccountId, t.Amount, t.Type.ToString(), t.CreatedAt))
            .ToList();
    }
}
