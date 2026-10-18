using LibraryLoans.Application.DTOs.Common;

namespace LibraryLoans.Application.DTOs.Member;

public class GetMemberDto : BaseDto
{
    public required string Name { get; set; }
    
    public static GetMemberDto From(Domain.Member member) => new()
    {
        Id = member.Id,
        Name = member.Name
    };
}