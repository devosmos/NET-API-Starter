using Devosmos.ApiStarter.Application.Abstractions.Operations;
using Devosmos.ApiStarter.Application.Common.Results;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Devosmos.ApiStarter.Application.Common.Operations;

internal sealed class OperationExecutor(IServiceProvider serviceProvider) : IOperationExecutor
{
    public async Task<Result<TResponse>> ExecuteAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : notnull
    {
        var validators = serviceProvider.GetServices<IValidator<TRequest>>();
        var validationFailures = new Dictionary<string, string[]>(StringComparer.Ordinal);

        foreach (var validator in validators)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            foreach (var group in validationResult.Errors.GroupBy(error => error.PropertyName))
            {
                validationFailures[group.Key] = group
                    .Select(error => error.ErrorMessage)
                    .Distinct(StringComparer.Ordinal)
                    .ToArray();
            }
        }

        if (validationFailures.Count > 0)
        {
            return Result<TResponse>.Failure(ApplicationError.Validation(validationFailures));
        }

        var handler = serviceProvider.GetRequiredService<IOperationHandler<TRequest, TResponse>>();
        return await handler.HandleAsync(request, cancellationToken);
    }
}
