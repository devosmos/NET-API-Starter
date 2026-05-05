namespace Devosmos.ApiStarter.Domain.Abstractions;

public interface IDomainEvent
{
    DateTimeOffset OccurredUtc { get; }
}
