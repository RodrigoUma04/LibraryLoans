using LibraryLoans.Application.Contracts.Persistence;
using LibraryLoans.Domain;

namespace LibraryLoans.Infrastructure.Repositories;

public class LoanRepository(LibraryLoansDbContext dbContext) : GenericRepository<Loan>(dbContext), ILoanRepository;