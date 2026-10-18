using LibraryLoans.Application.Contracts.Persistence;
using LibraryLoans.Domain;

namespace LibraryLoans.Infrastructure.Repositories;

public class BookRepository(LibraryLoansDbContext dbContext) : GenericRepository<Book>(dbContext), IBookRepository;