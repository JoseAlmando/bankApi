using BankApi.Application.Interfaces;
using BankApi.Infrastructure.Context;

namespace BankApi.Infrastructure.Repositories;

public class UnitOfWork(BankDbContext db) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => db.SaveChangesAsync(ct);
}
