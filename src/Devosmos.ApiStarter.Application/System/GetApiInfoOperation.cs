using Devosmos.ApiStarter.Application.Abstractions.Operations;
using Devosmos.ApiStarter.Application.Abstractions.Time;
using Devosmos.ApiStarter.Application.Common.Results;

namespace Devosmos.ApiStarter.Application.System;

internal sealed class GetApiInfoOperation(IDateTimeProvider dateTimeProvider)
    : IOperationHandler<GetApiInfoRequest, ApiInfoResponse>
{
    public Task<Result<ApiInfoResponse>> HandleAsync(
        GetApiInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = new ApiInfoResponse(
            "Devosmos.ApiStarter",
            "1.0.0",
            "net10.0",
            dateTimeProvider.UtcNow);

        return Task.FromResult(Result<ApiInfoResponse>.Success(response));
    }
}
