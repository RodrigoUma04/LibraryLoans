using LibraryLoans.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryLoans.Infrastructure.Configuration;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasIndex(b => b.Isbn).IsUnique();
        
        builder.Property(b => b.Title)
            .HasMaxLength(200);
        
        builder.Property(b => b.Author)
            .HasMaxLength(200);
        
        builder.Property(b => b.Isbn)
            .HasMaxLength(13)
            .IsFixedLength();
    }
}