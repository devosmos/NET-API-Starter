using Devosmos.ApiStarter.Application.Abstractions.Data;
using Devosmos.ApiStarter.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Devosmos.ApiStarter.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
