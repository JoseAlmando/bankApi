using BankApi.Domain;

namespace BankApi.Application.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Account>> ListByOwnerIdAsync(string ownerId, CancellationToken ct = default);
    Task AddAsync(Account account, CancellationToken ct = default);
    void Update(Account account);
    Task<bool> ExistsAccountNumberAsync(string accountNumber, CancellationToken ct = default);
}
