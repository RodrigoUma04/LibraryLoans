using LibraryLoans.Application.Contracts.Persistence;
using LibraryLoans.Application.DTOs.Member;
using LibraryLoans.Application.Validators;
using LibraryLoans.Domain;
using LibraryLoans.Domain.Common;

namespace LibraryLoans.Application.Services;

public class MemberService(IMemberRepository memberRepository, IUnitOfWork unitOfWork)
{
    public async Task<IReadOnlyCollection<GetMemberDto>> GetMembersAsync(CancellationToken cancellationToken = default)
    {
        var members = await memberRepository.GetAllAsync(cancellationToken);

        return [.. members.Select(GetMemberDto.From)];
    }

    public async Task<Result<GetMemberDto>> GetMemberAsync(int id, CancellationToken cancellationToken = default)
    {
        var member = await memberRepository.GetByIdAsync(id, cancellationToken);
        
        return member is null 
            ? Result<GetMemberDto>.Fail($"Member with id {id} not found", ErrorType.NotFound) 
            : Result<GetMemberDto>.Ok(GetMemberDto.From(member));
    }

    public async Task<Result<GetMemberDto>> AddMemberAsync(CreateMemberDto dto, CancellationToken cancellationToken = default)
    {
        var validation = CreateMemberDtoValidator.Validate(dto);
        if (validation.IsFailure)
            return Result<GetMemberDto>.Fail(validation.Errors, ErrorType.Validation);

        var member = new Member { Name = dto.Name };
        
        memberRepository.Add(member);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<GetMemberDto>.Ok(GetMemberDto.From(member));
    }
}