using LibraryLoans.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryLoans.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<BookService>();
        services.AddScoped<MemberService>();
        services.AddScoped<LoanService>();
        
        return services;
    }
}