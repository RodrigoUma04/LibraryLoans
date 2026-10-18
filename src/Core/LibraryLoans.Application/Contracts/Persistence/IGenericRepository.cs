using LibraryLoans.Domain.Common;

namespace LibraryLoans.Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(T entity);
    void Delete(T entity);
}