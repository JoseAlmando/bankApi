using BankApi.Application.Interfaces;
using BankApi.Domain;
using BankApi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Infrastructure.Repositories;

public class TransactionRepository(BankDbContext db) : ITransactionRepository
{
    public async Task AddAsync(Transaction transaction, CancellationToken ct = default)
        => await db.Transactions.AddAsync(transaction, ct);

    public async Task<IReadOnlyList<Transaction>> ListByAccountIdAsync(Guid accountId, CancellationToken ct = default)
        => await db.Transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(ct);
}
