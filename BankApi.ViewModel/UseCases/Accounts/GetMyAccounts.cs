using BankApi.Application.DTOs.Accounts;
using BankApi.Application.Interfaces;

namespace BankApi.Application.UseCases.Accounts;

public class GetMyAccounts(IAccountRepository accounts, ICurrentUserService currentUser)
{
    public async Task<IReadOnlyList<AccountResponse>> ExecuteAsync(CancellationToken ct = default)
    {
        var list = await accounts.ListByOwnerIdAsync(currentUser.UserId, ct);
        return list
            .Select(a => new AccountResponse(a.Id, a.AccountNumber, a.Balance, a.CreatedAt))
            .ToList();
    }
}
