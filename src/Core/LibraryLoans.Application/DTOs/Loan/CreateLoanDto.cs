namespace LibraryLoans.Application.DTOs.Loan;

public class CreateLoanDto
{
    public int BookId { get; set; }
    public int MemberId { get; set; }
}