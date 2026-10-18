using LibraryLoans.Application.Contracts.Persistence;
using LibraryLoans.Domain;
using LibraryLoans.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace LibraryLoans.Infrastructure;

public class LibraryLoansDbContext : DbContext, IUnitOfWork
{
    public LibraryLoansDbContext(DbContextOptions<LibraryLoansDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryLoansDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        DateTime now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>()
                     .Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            entry.Entity.DateModified = now;
            
            if(entry.State == EntityState.Added)
                entry.Entity.DateCreated = now;
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
    
    public DbSet<Book> Books => Set<Book>();                                                                                                                                                                                         
    public DbSet<Member> Members => Set<Member>();                                                                                                                                                                                   
    public DbSet<Loan> Loans => Set<Loan>(); 
}