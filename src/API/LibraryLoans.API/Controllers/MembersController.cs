using LibraryLoans.API.Extensions;
using LibraryLoans.Application.DTOs.Loan;
using LibraryLoans.Application.DTOs.Member;
using LibraryLoans.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryLoans.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController(MemberService memberService, LoanService loanService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<GetMemberDto>>> GetMembers(CancellationToken cancellationToken) =>
        Ok(await memberService.GetMembersAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetMemberDto>> GetMember(int id, CancellationToken cancellationToken)
    {
        var result = await memberService.GetMemberAsync(id, cancellationToken);

        if (result.IsFailure)
            return result.ToProblem();
        
        return Ok(result.Value);
    }

    [HttpGet("{id:int}/loans")]
    public async Task<ActionResult<IReadOnlyCollection<GetLoanDto>>> GetMemberLoans(int id,
        CancellationToken cancellationToken)
    {
        var result = await loanService.GetMemberLoansAsync(id, cancellationToken);
        
        if (result.IsFailure)
            return result.ToProblem();
        
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<GetMemberDto>> PostMember([FromBody] CreateMemberDto dto,
        CancellationToken cancellationToken)
    {
        var result = await memberService.AddMemberAsync(dto, cancellationToken);

        if (result.IsFailure)
            return result.ToProblem();

        return CreatedAtAction(nameof(GetMember), new { id = result.Value!.Id }, result.Value);
    }
}