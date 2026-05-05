using System.Data.Common;
using Devosmos.ApiStarter.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Respawn;
using Testcontainers.MsSql;

namespace Devosmos.ApiStarter.IntegrationTests.Infrastructure;

public sealed class ApiTestFactory : WebApplicationFactory<Program>, IAsyncDisposable
{
    private readonly MsSqlContainer _sqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    private Respawner? _respawner;

    public string ConnectionString => _sqlContainer.GetConnectionString();

    public async Task InitializeAsync()
    {
        using var startupTimeout = new CancellationTokenSource(TimeSpan.FromMinutes(3));
        await _sqlContainer.StartAsync(startupTimeout.Token);
    }

    public async Task ResetDatabaseAsync()
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        _respawner ??= await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            TablesToIgnore = ["__EFMigrationsHistory"]
        });

        await _respawner.ResetAsync(connection);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.EnsureCreated();

        return host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = ConnectionString,
                ["OpenApi:Enabled"] = "true",
                ["Authentication:Authority"] = "https://login.microsoftonline.com/test/v2.0",
                ["Authentication:Audience"] = "api://devosmos-api-starter-test"
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.RemoveAll<DbConnection>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(ConnectionString);
            });

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                    options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.SchemeName,
                    _ => { });
        });
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _sqlContainer.DisposeAsync();
    }
}
