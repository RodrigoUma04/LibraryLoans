using LibraryLoans.API.Extensions;
using LibraryLoans.Application.DTOs.Loan;
using LibraryLoans.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryLoans.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController(LoanService loanService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<GetLoanDto>> Borrow(CreateLoanDto dto, CancellationToken cancellationToken)
    {
        var result = await loanService.BorrowAsync(dto, cancellationToken);

        if (result.IsFailure)
            return result.ToProblem();

        return StatusCode(StatusCodes.Status201Created, result.Value);
    }

    [HttpPost("{id:int}/return")]
    public async Task<ActionResult<GetLoanDto>> Return(int id, CancellationToken cancellationToken)
    {
        var result = await loanService.ReturnAsync(id, cancellationToken);

        if (result.IsFailure)
            return result.ToProblem();

        return Ok(result.Value);
    }
}