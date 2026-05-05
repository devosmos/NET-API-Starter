---
applyTo: "src/Devosmos.ApiStarter.Application/**/*.cs"
---

# Application Layer Instructions

- Use `IOperationHandler<TRequest,TResponse>` for use cases.
- Use FluentValidation validators next to request types.
- Return `Result<T>` instead of throwing for expected business failures.
- Depend only on `Domain` and abstractions.
- Define interfaces for infrastructure capabilities here.
- Keep DTOs independent from EF Core and ASP.NET Core.
