using LibraryLoans.Application.DTOs.Loan;
using LibraryLoans.Domain;

namespace LibraryLoans.Application.Contracts.Persistence;

public interface ILoanRepository : IGenericRepository<Loan>
{
    Task<bool> IsBookBorrowedAsync(int bookId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Loan>> GetActiveLoansByMemberAsync(int memberId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<MemberLoanDto>> GetLoansByMemberAsync(int memberId, CancellationToken cancellationToken = default);
}