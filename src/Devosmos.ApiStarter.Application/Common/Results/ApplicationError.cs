namespace Devosmos.ApiStarter.Application.Common.Results;

public sealed record ApplicationError(
    string Code,
    string Message,
    ApplicationErrorType Type,
    IReadOnlyDictionary<string, string[]>? ValidationErrors = null)
{
    public static ApplicationError Validation(IReadOnlyDictionary<string, string[]> errors)
    {
        return new ApplicationError(
            "validation.failed",
            "One or more validation errors occurred.",
            ApplicationErrorType.Validation,
            errors);
    }

    public static ApplicationError Failure(string code, string message)
    {
        return new ApplicationError(code, message, ApplicationErrorType.Failure);
    }

    public static ApplicationError NotFound(string code, string message)
    {
        return new ApplicationError(code, message, ApplicationErrorType.NotFound);
    }

    public static ApplicationError Conflict(string code, string message)
    {
        return new ApplicationError(code, message, ApplicationErrorType.Conflict);
    }
}
