using LibraryLoans.Domain.Common;

namespace LibraryLoans.Domain;

public class Book : BaseEntity
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Isbn { get; set; }
}