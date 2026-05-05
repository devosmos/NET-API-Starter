using Devosmos.ApiStarter.Application.Abstractions.Data;
using Devosmos.ApiStarter.Application.Abstractions.Security;
using Devosmos.ApiStarter.Application.Abstractions.Time;
using Devosmos.ApiStarter.Infrastructure.Persistence;
using Devosmos.ApiStarter.Infrastructure.Security;
using Devosmos.ApiStarter.Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Devosmos.ApiStarter.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Default");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = "Server=localhost,1433;Database=DevosmosApiStarter;User Id=sa;Password=Your_strong_password123;Encrypt=True;TrustServerCertificate=True;";
            }

            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IAppDbContext>(serviceProvider => serviceProvider.GetRequiredService<AppDbContext>());
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>("database");

        return services;
    }
}
