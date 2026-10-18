using LibraryLoans.Application.DTOs.Member;
using LibraryLoans.Domain.Common;

namespace LibraryLoans.Application.Validators;

public static class CreateMemberDtoValidator
{
    private const int MaxNameLength = 200;
    
    public static Result Validate(CreateMemberDto dto)
    {
        List<string> errors = [];
        
        if (string.IsNullOrWhiteSpace(dto.Name))
            errors.Add($"{nameof(dto.Name)} is required");
        else if (dto.Name.Length > MaxNameLength)
            errors.Add($"{nameof(dto.Name)} must not be longer than {MaxNameLength} characters");
        
        return errors.Count == 0 ? Result.Ok() : Result.Fail(errors, ErrorType.Validation);
    }
}