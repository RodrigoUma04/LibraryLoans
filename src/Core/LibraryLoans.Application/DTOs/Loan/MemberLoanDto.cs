namespace LibraryLoans.Application.DTOs.Loan;

public class MemberLoanDto
{
    public int LoanId { get; set; }
    public int BookId { get; set; }
    public required string BookTitle { get; set; }
    public DateTime DateOfLoan { get; set; }
    public DateTime DateOfExpiration { get; set; }
    public DateTime? DateOfReturn { get; set; }
}