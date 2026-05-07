using BankApi.Application.DTOs.Accounts;
using BankApi.Application.Interfaces;

namespace BankApi.Application.UseCases.Transactions;

public class Deposit(
    IAccountRepository accounts,
    ITransactionRepository transactions,
    IUnitOfWork uow,
    ICurrentUserService currentUser)
{
    public async Task<AccountResponse> ExecuteAsync(Guid accountId, decimal amount, CancellationToken ct = default)
    {
        var account = await accounts.GetByIdAsync(accountId, ct)
            ?? throw new KeyNotFoundException($"Cuenta {accountId} no encontrada.");

        if (account.OwnerId != currentUser.UserId)
            throw new UnauthorizedAccessException("No tienes acceso a esta cuenta.");

        account.Deposit(amount);

        var newTx = account.Transactions.Last();
        await transactions.AddAsync(newTx, ct);

        accounts.Update(account);
        await uow.SaveChangesAsync(ct);

        return new AccountResponse(account.Id, account.AccountNumber, account.Balance, account.CreatedAt);
    }
}
