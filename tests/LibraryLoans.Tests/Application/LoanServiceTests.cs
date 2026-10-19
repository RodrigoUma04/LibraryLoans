using LibraryLoans.Application.Contracts.Persistence;
using LibraryLoans.Application.DTOs.Loan;
using LibraryLoans.Application.Services;
using LibraryLoans.Domain;
using LibraryLoans.Domain.Common;
using NSubstitute;

namespace LibraryLoans.Tests.Application;

public class LoanServiceTests
{
    private const int BookId = 1;
    private const int MemberId = 2;

    private ILoanRepository _loanRepository = null!;
    private IBookRepository _bookRepository = null!;
    private IMemberRepository _memberRepository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private LoanService _service = null!;

    private static readonly CreateLoanDto Dto = new() { BookId = BookId, MemberId = MemberId };

    [SetUp]
    public void SetUp()
    {
        _loanRepository = Substitute.For<ILoanRepository>();
        _bookRepository = Substitute.For<IBookRepository>();
        _memberRepository = Substitute.For<IMemberRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        // Happy-path defaults; each test overrides only what it is about
        _memberRepository.GetByIdAsync(MemberId, Arg.Any<CancellationToken>()).Returns(new Member { Name = "Ada" });
        _bookRepository.GetByIdAsync(BookId, Arg.Any<CancellationToken>())
            .Returns(new Book { Title = "Clean Code", Author = "Robert C. Martin", Isbn = "9780132350884" });
        _loanRepository.IsBookBorrowedAsync(BookId, Arg.Any<CancellationToken>()).Returns(false);
        _loanRepository.GetActiveLoansByMemberAsync(MemberId, Arg.Any<CancellationToken>()).Returns([]);

        _service = new LoanService(_loanRepository, _bookRepository, _memberRepository, _unitOfWork);
    }

    [Test]
    public async Task BorrowAsync_AllRulesPass_AddsLoanAndSavesOnce()
    {
        var result = await _service.BorrowAsync(Dto);

        Assert.That(result.IsSuccess);
        _loanRepository.Received(1).Add(Arg.Is<Loan>(l => l.BookId == BookId && l.MemberId == MemberId));
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task BorrowAsync_MemberNotFound_FailsWithNotFoundAndDoesNotSave()
    {
        _memberRepository.GetByIdAsync(MemberId, Arg.Any<CancellationToken>()).Returns((Member?)null);

        var result = await _service.BorrowAsync(Dto);

        Assert.That(result.ErrorType, Is.EqualTo(ErrorType.NotFound));
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task BorrowAsync_BookAlreadyBorrowed_FailsWithConflict()
    {
        _loanRepository.IsBookBorrowedAsync(BookId, Arg.Any<CancellationToken>()).Returns(true);

        var result = await _service.BorrowAsync(Dto);

        Assert.That(result.ErrorType, Is.EqualTo(ErrorType.Conflict));
        _loanRepository.DidNotReceive().Add(Arg.Any<Loan>());
    }

    [Test]
    public async Task BorrowAsync_MemberHasFiveActiveLoans_FailsWithConflict()
    {
        var fiveLoans = Enumerable.Range(10, 5).Select(id => Loan.Create(id, MemberId, DateTime.UtcNow)).ToList();
        _loanRepository.GetActiveLoansByMemberAsync(MemberId, Arg.Any<CancellationToken>()).Returns(fiveLoans);

        var result = await _service.BorrowAsync(Dto);

        Assert.That(result.ErrorType, Is.EqualTo(ErrorType.Conflict));
        Assert.That(result.Errors, Has.Some.Contains("5"));
    }

    [Test]
    public async Task BorrowAsync_MemberHasOverdueLoan_FailsWithConflict()
    {
        var overdue = Loan.Create(10, MemberId, DateTime.UtcNow.AddDays(-30));
        _loanRepository.GetActiveLoansByMemberAsync(MemberId, Arg.Any<CancellationToken>()).Returns([overdue]);

        var result = await _service.BorrowAsync(Dto);

        Assert.That(result.ErrorType, Is.EqualTo(ErrorType.Conflict));
        Assert.That(result.Errors, Has.Some.Contains("overdue"));
    }
}
