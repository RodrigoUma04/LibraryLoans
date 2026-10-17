using LibraryLoans.Domain.Common;

namespace LibraryLoans.Domain;

public class Member : BaseEntity
{
    public required string Name { get; set; }
}