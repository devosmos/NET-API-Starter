namespace Devosmos.ApiStarter.Application.Abstractions.Security;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }

    string? UserId { get; }

    string? UserName { get; }
}
