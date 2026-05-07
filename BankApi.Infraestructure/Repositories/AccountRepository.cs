using BankApi.Application.Interfaces;
using BankApi.Domain;
using BankApi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Infrastructure.Repositories;

public class AccountRepository(BankDbContext db) : IAccountRepository
{
    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await db.Accounts.FindAsync([id], ct);

    public async Task<IReadOnlyList<Account>> ListByOwnerIdAsync(string ownerId, CancellationToken ct = default)
        => await db.Accounts
            .Where(a => a.OwnerId == ownerId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(ct);

    public async Task AddAsync(Account account, CancellationToken ct = default)
        => await db.Accounts.AddAsync(account, ct);

    public void Update(Account account)
        => db.Accounts.Update(account);

    public async Task<bool> ExistsAccountNumberAsync(string accountNumber, CancellationToken ct = default)
        => await db.Accounts.AnyAsync(a => a.AccountNumber == accountNumber, ct);
}
