using LibraryLoans.Domain;

namespace LibraryLoans.Application.Contracts.Persistence;

public interface IBookRepository : IGenericRepository<Book>
{
    Task<bool> IsbnExistsAsync(string isbn, CancellationToken cancellationToken = default);
}