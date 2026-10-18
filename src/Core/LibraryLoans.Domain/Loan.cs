using LibraryLoans.Domain.Common;

namespace LibraryLoans.Domain;

public class Loan : BaseEntity
{
    public int BookId { get; private set; }
    public Book? Book { get; private set; }
    public int MemberId { get; private set; }
    public Member? Member { get; private set; }
    public DateTime DateOfLoan { get; private set; }
    public DateTime DateOfExpiration { get; private set; }
    public DateTime? DateOfReturn { get; private set; }

    private Loan() {}

    public static Loan Create(int bookId, int memberId, DateTime now)
    {
        return new Loan
        {
            BookId = bookId,
            MemberId = memberId,
            DateOfLoan = now,
            DateOfExpiration = now.AddDays(21)
        };
    }

    public Result Return(DateTime returnDate)
    {
        if (DateOfReturn is not null)
            return Result.Fail("Book already returned", ErrorType.Conflict);

        if (returnDate < DateOfLoan)
            return Result.Fail("A book can only be returned after the loan date", ErrorType.Validation);
        
        DateOfReturn = returnDate;
        return Result.Ok();
    }
    
    public bool IsOverdue(DateTime now) =>
        DateOfReturn is null && now > DateOfExpiration;
}