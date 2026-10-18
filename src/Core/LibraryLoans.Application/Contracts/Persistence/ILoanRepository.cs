using LibraryLoans.Domain;

namespace LibraryLoans.Application.Contracts.Persistence;

public interface ILoanRepository : IGenericRepository<Loan>
{
    Task<bool> IsBookBorrowedAsync(int bookId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Loan>> GetActiveLoansByMemberAsync(int memberId, CancellationToken cancellationToken = default);
}