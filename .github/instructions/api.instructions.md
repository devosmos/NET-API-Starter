---
applyTo: "src/Devosmos.ApiStarter.Api/**/*.cs"
---

# API Layer Instructions

- Controllers must be thin and call `IOperationExecutor`.
- Use `[ApiController]`, `[ApiVersion]`, and `api/v{version:apiVersion}/[controller]`.
- Controllers are authorized by fallback policy; add `[AllowAnonymous]` only for deliberate public endpoints.
- Return `IActionResult` through `Result<T>.ToActionResult(this)` for operation outcomes.
- Add explicit `ProducesResponseType` attributes for success, validation, expected errors, 401, and 403.
- Keep middleware registration in `Program.cs`.
