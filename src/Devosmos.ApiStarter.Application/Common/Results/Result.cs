namespace Devosmos.ApiStarter.Application.Common.Results;

public sealed class Result<T>
{
    private Result(T value)
    {
        Value = value;
    }

    private Result(ApplicationError error)
    {
        Error = error;
    }

    public bool IsSuccess => Error is null;

    public T? Value { get; }

    public ApplicationError? Error { get; }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public static Result<T> Failure(ApplicationError error)
    {
        return new Result<T>(error);
    }
}
