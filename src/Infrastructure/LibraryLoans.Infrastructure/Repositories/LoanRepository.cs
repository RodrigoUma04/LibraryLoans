using LibraryLoans.Application.Contracts.Persistence;
using LibraryLoans.Application.DTOs.Loan;
using LibraryLoans.Domain;
using Microsoft.EntityFrameworkCore;

namespace LibraryLoans.Infrastructure.Repositories;

public class LoanRepository(LibraryLoansDbContext dbContext) : GenericRepository<Loan>(dbContext), ILoanRepository
{
    public async Task<bool> IsBookBorrowedAsync(int bookId, CancellationToken cancellationToken = default) =>
        await DbContext.Loans.AnyAsync(l => l.BookId == bookId && l.DateOfReturn == null, cancellationToken);

    public async Task<IReadOnlyCollection<Loan>> GetActiveLoansByMemberAsync(int memberId, CancellationToken cancellationToken = default) =>
        await DbContext.Loans.AsNoTracking()
            .Where(l => l.MemberId == memberId && l.DateOfReturn == null)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<MemberLoanDto>> GetLoansByMemberAsync(int memberId, CancellationToken cancellationToken = default) =>
        await DbContext.Loans
            .Where(l => l.MemberId == memberId)
            .OrderByDescending(l => l.DateOfLoan)
            .Select(l => new MemberLoanDto
            {
                LoanId = l.Id,
                BookId = l.BookId,
                BookTitle = l.Book!.Title,
                DateOfLoan =  l.DateOfLoan,
                DateOfExpiration =  l.DateOfExpiration,
                DateOfReturn = l.DateOfReturn,
            })
            .ToListAsync(cancellationToken);
}