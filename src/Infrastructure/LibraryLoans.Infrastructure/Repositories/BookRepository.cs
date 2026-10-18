using LibraryLoans.Application.Contracts.Persistence;
using LibraryLoans.Domain;
using Microsoft.EntityFrameworkCore;

namespace LibraryLoans.Infrastructure.Repositories;

public class BookRepository(LibraryLoansDbContext dbContext) : GenericRepository<Book>(dbContext), IBookRepository
{
    public async Task<bool> IsbnExistsAsync(string isbn, CancellationToken cancellationToken = default) =>
        await DbContext.Books.AnyAsync(b => b.Isbn == isbn, cancellationToken);
}