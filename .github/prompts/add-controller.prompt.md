# Add Controller Prompt

Add an ASP.NET Core controller endpoint for the requested real business operation.

Requirements:

- Put the controller in `src/Devosmos.ApiStarter.Api/Controllers`.
- Use `[ApiController]`, `[ApiVersion(1.0)]`, and route `api/v{version:apiVersion}/[controller]`.
- Keep the controller thin.
- Inject `IOperationExecutor`.
- Call the matching application operation request.
- Return `result.ToActionResult(this)`.
- Add `ProducesResponseType` attributes for 200/201/204, 400, expected errors, 401, and 403.
- Add or update integration tests.

Before coding, identify the operation request/response and route shape.
