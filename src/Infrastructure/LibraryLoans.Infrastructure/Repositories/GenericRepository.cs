using LibraryLoans.Application.Contracts.Persistence;
using LibraryLoans.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace LibraryLoans.Infrastructure.Repositories;

public abstract class GenericRepository<T>(LibraryLoansDbContext dbContext) : IGenericRepository<T> where T : BaseEntity
{
    protected readonly LibraryLoansDbContext DbContext = dbContext;
    
    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await DbContext.FindAsync<T>([id], cancellationToken);

    public async Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await DbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);

    public void Add(T entity) => DbContext.Add(entity);
}