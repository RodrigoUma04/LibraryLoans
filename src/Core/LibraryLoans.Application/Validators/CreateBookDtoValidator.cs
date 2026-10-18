using LibraryLoans.Application.DTOs.Book;
using LibraryLoans.Domain.Common;

namespace LibraryLoans.Application.Validators;

public static class CreateBookDtoValidator
{
    private const int MaxTitleLength = 200;
    private const int MaxAuthorLength = 200;
    private const int IsbnLength = 13;

    public static Result Validate(CreateBookDto dto)
    {
        List<string> errors = [];

        if (string.IsNullOrWhiteSpace(dto.Title))
            errors.Add($"{nameof(dto.Title)} is required");
        else if (dto.Title.Length > MaxTitleLength)
            errors.Add($"{nameof(dto.Title)} must not be longer than {MaxTitleLength} characters");

        if (string.IsNullOrWhiteSpace(dto.Author))
            errors.Add($"{nameof(dto.Author)} is required");
        else if (dto.Author.Length > MaxAuthorLength)
            errors.Add($"{nameof(dto.Author)} must not be longer than {MaxAuthorLength} characters");

        if (dto.Isbn.Length != IsbnLength || !dto.Isbn.All(char.IsAsciiDigit))
            errors.Add($"{nameof(dto.Isbn)} must be exactly {IsbnLength} digits");

        return errors.Count == 0 ? Result.Ok() : Result.Fail(errors, ErrorType.Validation);
    }
}