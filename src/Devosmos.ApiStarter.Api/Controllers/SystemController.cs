using Asp.Versioning;
using Devosmos.ApiStarter.Api.Http;
using Devosmos.ApiStarter.Application.Abstractions.Operations;
using Devosmos.ApiStarter.Application.System;
using Microsoft.AspNetCore.Mvc;

namespace Devosmos.ApiStarter.Api.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class SystemController(IOperationExecutor operationExecutor) : ControllerBase
{
    [HttpGet("info")]
    [ProducesResponseType<ApiInfoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetInfo(CancellationToken cancellationToken)
    {
        var result = await operationExecutor.ExecuteAsync<GetApiInfoRequest, ApiInfoResponse>(
            new GetApiInfoRequest(),
            cancellationToken);

        return result.ToActionResult(this);
    }
}
