namespace Devosmos.ApiStarter.Domain.Errors;

public sealed record DomainError(string Code, string Message)
{
    public static DomainError InvariantViolation(string code, string message)
    {
        return new DomainError(code, message);
    }
}
