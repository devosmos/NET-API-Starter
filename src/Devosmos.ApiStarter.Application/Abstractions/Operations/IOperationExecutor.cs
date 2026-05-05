using Devosmos.ApiStarter.Application.Common.Results;

namespace Devosmos.ApiStarter.Application.Abstractions.Operations;

public interface IOperationExecutor
{
    Task<Result<TResponse>> ExecuteAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : notnull;
}
