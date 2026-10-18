using LibraryLoans.Domain;
using LibraryLoans.Domain.Common;

namespace LibraryLoans.Tests.Domain;

public class LoanTests
{
    private const int BookId = 1;                                                                                                                                                                                                
    private const int MemberId = 1;                                                                                                                                                                                              
    private static readonly DateTime LoanDate = new(2026, 10, 1);
    private Loan _loan = null!;

    [SetUp]
    public void SetUp()
    {
        _loan = Loan.Create(BookId, MemberId, LoanDate);
    }
    
    [Test]
    public void Create_SetsExpirationTo21DaysAfterLoan()
    {
        Assert.That(_loan.DateOfExpiration, Is.EqualTo(LoanDate.AddDays(21)));
    }

    [Test]
    public void Return_SetsDateOfReturnToReturnDate()
    {
        DateTime returnDate = LoanDate.AddDays(7);
        
        Result result = _loan.Return(returnDate);
        
        Assert.That(_loan.DateOfReturn, Is.EqualTo(returnDate));
        Assert.That(result.IsSuccess);
    }

    [Test]
    public void Return_AlreadyReturned_Fails()
    {
        DateTime returnDate = LoanDate.AddDays(7);
        
        _loan.Return(returnDate);
        
        Result result = _loan.Return(LoanDate.AddDays(10));
        
        Assert.That(result.IsFailure);
        Assert.That(result.Error, Does.Contain("already returned"));
    }

    [Test]
    public void Return_ReturnDateBeforeLoanDate_Fails()
    {
        DateTime returnDate = LoanDate.Subtract(TimeSpan.FromDays(2));
        
        Result result = _loan.Return(returnDate);
        
        Assert.That(result.IsFailure);
        Assert.That(result.Error, Does.Contain("after the loan date"));
    }
    
    [Test]
    public void IsOverdue_AlreadyReturned_ReturnsFalse()
    {
        DateTime returnDate = LoanDate.AddDays(7);
        
        _loan.Return(returnDate);
        
        Assert.That(_loan.IsOverdue(returnDate.AddDays(2)), Is.False);
    }

    [TestCase(20, false)]
    [TestCase(21, false)]
    [TestCase(22, true)]
    public void IsOverdue_DependsOnDaysSinceLoan(int days, bool expected)
    {
        Assert.That(_loan.IsOverdue(LoanDate.AddDays(days)), Is.EqualTo(expected));
    }
}