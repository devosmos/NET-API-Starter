# Copilot Instructions

You are working in `Devosmos.ApiStarter`, a domain-neutral ASP.NET Core 10 API starter.

## Core Rules

- Preserve Clean Architecture dependencies.
- Do not add fake business domains.
- Keep controllers thin and authorized by default.
- Add use cases as operations in `Application`.
- Put domain invariants in `Domain`.
- Put EF Core and Azure implementations in `Infrastructure`.
- Return expected failures with `Result<T>` and map them to `ProblemDetails`.
- Use FluentValidation for request validation.
- Use xUnit v3, FluentAssertions 7.2.x, NSubstitute, Testcontainers.MsSql, and Respawn.
- Do not apply migrations on startup.
- Generate idempotent SQL migration scripts for review.

## File Placement

- Controller: `src/Devosmos.ApiStarter.Api/Controllers`
- Operation request/response/handler/validator: `src/Devosmos.ApiStarter.Application/<Feature>`
- Domain model: `src/Devosmos.ApiStarter.Domain/<Feature>`
- EF configuration: `src/Devosmos.ApiStarter.Infrastructure/Persistence/Configurations`
- Migrations: `src/Devosmos.ApiStarter.Infrastructure/Persistence/Migrations`
- Unit tests: `tests/Devosmos.ApiStarter.UnitTests`
- Integration tests: `tests/Devosmos.ApiStarter.IntegrationTests`

## Style

- Prefer records for immutable DTOs.
- Use cancellation tokens.
- Avoid reflection unless the existing code already uses it.
- Keep comments rare and useful.
- Add tests with the change.
