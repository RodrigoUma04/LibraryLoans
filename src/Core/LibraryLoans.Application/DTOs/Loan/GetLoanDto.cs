using LibraryLoans.Application.DTOs.Common;

namespace LibraryLoans.Application.DTOs.Loan;

public class GetLoanDto : BaseDto
{
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime DateOfLoan { get; set; }
    public DateTime DateOfExpiration { get; set; }
    public DateTime? DateOfReturn { get; set; }

    public static GetLoanDto From(Domain.Loan loan) => new()
    {
        Id = loan.Id,
        BookId = loan.BookId,
        DateOfExpiration = loan.DateOfExpiration,
        DateOfLoan = loan.DateOfLoan,
        DateOfReturn = loan.DateOfReturn,
        MemberId = loan.MemberId
    };
}