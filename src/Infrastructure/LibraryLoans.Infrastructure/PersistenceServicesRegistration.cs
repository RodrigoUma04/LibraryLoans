using LibraryLoans.Application.Contracts.Persistence;
using LibraryLoans.Domain;
using LibraryLoans.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryLoans.Infrastructure;

public static class PersistenceServicesRegistration
{
    public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<LibraryLoansDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("LibraryLoansConnectionString")));
        
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<LibraryLoansDbContext>());

        return services;
    }
}