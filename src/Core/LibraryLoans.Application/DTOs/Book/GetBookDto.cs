using LibraryLoans.Application.DTOs.Common;

namespace LibraryLoans.Application.DTOs.Book;

public class GetBookDto : BaseDto
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Isbn { get; set; }

    public static GetBookDto From(Domain.Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author,
        Isbn = book.Isbn
    };
}
