using Devosmos.ApiStarter.Application.Common.Results;

namespace Devosmos.ApiStarter.Application.Abstractions.Operations;

public interface IOperationHandler<in TRequest, TResponse>
    where TRequest : notnull
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
