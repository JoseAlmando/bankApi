using BankApi.Application.DTOs.Accounts;
using BankApi.Application.Interfaces;
using BankApi.Domain;

namespace BankApi.Application.UseCases.Accounts;

public class CreateAccount(
    IAccountRepository accounts,
    IUnitOfWork uow,
    ICurrentUserService currentUser)
{
    public async Task<AccountResponse> ExecuteAsync(CreateAccountRequest request, CancellationToken ct = default)
    {
        var accountNumber = GenerateAccountNumber();
        while (await accounts.ExistsAccountNumberAsync(accountNumber, ct))
            accountNumber = GenerateAccountNumber();

        var account = new Account(currentUser.UserId, accountNumber, request.InitialBalance);

        await accounts.AddAsync(account, ct);
        await uow.SaveChangesAsync(ct);

        return new AccountResponse(account.Id, account.AccountNumber, account.Balance, account.CreatedAt);
    }

    private static string GenerateAccountNumber()
        => $"ACC-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
}
