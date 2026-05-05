using Devosmos.ApiStarter.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Devosmos.ApiStarter.Infrastructure.Persistence.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");

        builder.HasKey(message => message.Id);

        builder.Property(message => message.Type)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(message => message.Content)
            .IsRequired();

        builder.Property(message => message.Error)
            .HasMaxLength(2048);

        builder.HasIndex(message => message.ProcessedUtc);
    }
}
