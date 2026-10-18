using LibraryLoans.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryLoans.Infrastructure.Configuration;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.HasOne(l => l.Book).WithMany().HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(l => l.Member).WithMany().HasForeignKey(l => l.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(l => l.BookId)
            .IsUnique()
            .HasFilter($"{nameof(Loan.DateOfReturn)} IS NULL");
    }
}