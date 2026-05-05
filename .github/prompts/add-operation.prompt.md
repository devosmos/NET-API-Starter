# Add Operation Prompt

Create a use-case operation for a real feature.

Requirements:

- Add request, response, validator, and handler under `src/Devosmos.ApiStarter.Application/<Feature>`.
- Handler implements `IOperationHandler<TRequest,TResponse>`.
- Use `Result<T>` for success and expected failures.
- Use application interfaces for infrastructure concerns.
- Keep DTOs free of ASP.NET Core and EF Core types.
- Add unit tests for success, validation, and expected failure paths.

Do not invent sample business behavior. Ask for the real business rule if it is missing.
