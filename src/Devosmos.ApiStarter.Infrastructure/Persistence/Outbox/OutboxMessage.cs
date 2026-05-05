namespace Devosmos.ApiStarter.Infrastructure.Persistence.Outbox;

public sealed class OutboxMessage
{
    private OutboxMessage()
    {
    }

    public OutboxMessage(Guid id, DateTimeOffset occurredUtc, string type, string content)
    {
        Id = id;
        OccurredUtc = occurredUtc;
        Type = type;
        Content = content;
    }

    public Guid Id { get; private set; }

    public DateTimeOffset OccurredUtc { get; private set; }

    public string Type { get; private set; } = string.Empty;

    public string Content { get; private set; } = string.Empty;

    public DateTimeOffset? ProcessedUtc { get; private set; }

    public string? Error { get; private set; }

    public void MarkProcessed(DateTimeOffset processedUtc)
    {
        ProcessedUtc = processedUtc;
        Error = null;
    }

    public void MarkFailed(string error)
    {
        Error = error;
    }
}
