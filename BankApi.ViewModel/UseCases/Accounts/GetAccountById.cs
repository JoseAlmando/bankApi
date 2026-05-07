using BankApi.Application.DTOs.Accounts;
using BankApi.Application.Interfaces;

namespace BankApi.Application.UseCases.Accounts;

public class GetAccountById(IAccountRepository accounts, ICurrentUserService currentUser)
{
    public async Task<AccountResponse> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var account = await accounts.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Cuenta {id} no encontrada.");

        if (account.OwnerId != currentUser.UserId)
            throw new UnauthorizedAccessException("No tienes acceso a esta cuenta.");

        return new AccountResponse(account.Id, account.AccountNumber, account.Balance, account.CreatedAt);
    }
}
