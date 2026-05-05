using Devosmos.ApiStarter.Application.Abstractions.Time;

namespace Devosmos.ApiStarter.Infrastructure.Time;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
