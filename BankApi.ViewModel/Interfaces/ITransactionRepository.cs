using BankApi.Domain;

namespace BankApi.Application.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken ct = default);
    Task<IReadOnlyList<Transaction>> ListByAccountIdAsync(Guid accountId, CancellationToken ct = default);
}
