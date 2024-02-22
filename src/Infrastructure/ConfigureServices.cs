using UrlShortenerService.Application.Common.Interfaces;
using UrlShortenerService.Infrastructure.Persistence;
using UrlShortenerService.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UrlShortenerService.Infrastructure.Services;

namespace Microsoft.Extensions.DependencyInjection;
// see: Entity Framework Core
// EF Core postgres examples
// ASPNET.core -- collection of .NET classes for web applications (Active Server Pages) cf. PHP

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            _ = services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("UrlShortenerServiceDb"));
        }
        else
        {
            _ = services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        _ = services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        _ = services.AddScoped<ApplicationDbContextInitialiser>();

        _ = services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}
