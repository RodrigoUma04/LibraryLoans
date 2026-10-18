using LibraryLoans.Application.Contracts.Persistence;
using LibraryLoans.Domain;
using Microsoft.EntityFrameworkCore;

namespace LibraryLoans.Infrastructure.Repositories;

public class LoanRepository(LibraryLoansDbContext dbContext) : GenericRepository<Loan>(dbContext), ILoanRepository
{
    public async Task<bool> IsBookBorrowedAsync(int bookId, CancellationToken cancellationToken = default) =>
        await DbContext.Loans.AnyAsync(l => l.BookId == bookId && l.DateOfReturn == null, cancellationToken);

    public async Task<IReadOnlyList<Loan>> GetActiveLoansByMemberAsync(int memberId, CancellationToken cancellationToken = default) =>
        await DbContext.Loans.AsNoTracking()
            .Where(l => l.MemberId == memberId && l.DateOfReturn == null)
            .ToListAsync(cancellationToken);
}