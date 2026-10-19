using LibraryLoans.Application.Contracts.Persistence;
using LibraryLoans.Application.DTOs.Loan;
using LibraryLoans.Domain;
using LibraryLoans.Domain.Common;

namespace LibraryLoans.Application.Services;

public class LoanService(ILoanRepository loanRepository, IBookRepository bookRepository, IMemberRepository memberRepository, IUnitOfWork unitOfWork)
{
    private const int MaxActiveLoans = 5;

    public async Task<Result<IReadOnlyCollection<MemberLoanDto>>> GetMemberLoansAsync(int memberId,
        CancellationToken cancellationToken = default)
    {
        var member = await memberRepository.GetByIdAsync(memberId, cancellationToken);
        if (member is null)
            return Result<IReadOnlyCollection<MemberLoanDto>>.Fail($"Member with id {memberId} not found", ErrorType.NotFound);
        
        var loans = await loanRepository.GetLoansByMemberAsync(memberId, cancellationToken);
        
        return Result<IReadOnlyCollection<MemberLoanDto>>.Ok(loans);
    }
    
    public async Task<Result<GetLoanDto>> BorrowAsync(CreateLoanDto dto, CancellationToken cancellationToken = default)
    {
        var member = await memberRepository.GetByIdAsync(dto.MemberId, cancellationToken);
        if (member is null)
            return Result<GetLoanDto>.Fail($"Member with id {dto.MemberId} not found", ErrorType.NotFound);
        
        var book = await bookRepository.GetByIdAsync(dto.BookId, cancellationToken);
        if (book is null)
            return Result<GetLoanDto>.Fail($"Book with id {dto.BookId} not found", ErrorType.NotFound);
        
        if (await loanRepository.IsBookBorrowedAsync(dto.BookId, cancellationToken))
            return Result<GetLoanDto>.Fail($"Book with id {dto.BookId} is already borrowed at this time", ErrorType.Conflict);

        var activeLoans = await loanRepository.GetActiveLoansByMemberAsync(dto.MemberId, cancellationToken);
        
        if (activeLoans.Count >= MaxActiveLoans)
            return Result<GetLoanDto>.Fail($"Member with id {dto.MemberId} already has {MaxActiveLoans} or more books borrowed", ErrorType.Conflict);
        
        var now = DateTime.UtcNow;
        if (activeLoans.Any(l => l.IsOverdue(now)))
            return Result<GetLoanDto>.Fail($"Member with id {dto.MemberId} cannot borrow due to an overdue loan", ErrorType.Conflict);

        var loan = Loan.Create(dto.BookId, dto.MemberId, now);
        
        loanRepository.Add(loan);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<GetLoanDto>.Ok(GetLoanDto.From(loan));
    }

    public async Task<Result<GetLoanDto>> ReturnAsync(int loanId, CancellationToken cancellationToken = default)
    {
        var loan = await loanRepository.GetByIdAsync(loanId, cancellationToken);
        if (loan is null)
            return Result<GetLoanDto>.Fail($"Loan with id {loanId} not found", ErrorType.NotFound);
        
        var result = loan.Return(DateTime.UtcNow);
        if(result.IsFailure)
            return Result<GetLoanDto>.Fail(result.Errors, result.ErrorType!.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<GetLoanDto>.Ok(GetLoanDto.From(loan));
    }
}