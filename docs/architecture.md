# Architecture

`Devosmos.ApiStarter` is a domain-neutral starter for controller-based ASP.NET Core 10 APIs on Azure.

## Layers

- `Devosmos.ApiStarter.Domain`: domain primitives, aggregate base types, value objects, domain events, and domain errors. It has no project dependencies.
- `Devosmos.ApiStarter.Application`: operation handlers, DTOs, FluentValidation validators, result/error modeling, and application-facing interfaces.
- `Devosmos.ApiStarter.Infrastructure`: EF Core `AppDbContext`, Azure SQL provider, unit of work, Key Vault configuration, current user, and clock implementations.
- `Devosmos.ApiStarter.Api`: controllers, authentication, authorization, versioning, OpenAPI, health checks, CORS, rate limiting, security headers, and dependency composition.

## Request Flow

1. Controller receives a versioned API request.
2. ASP.NET Core model binding and FluentValidation validate inputs.
3. Controller invokes `IOperationExecutor`.
4. `Application` operation returns `Result<T>`.
5. API maps success to HTTP 2xx and expected failures to `ProblemDetails`.
6. Infrastructure commits through `IUnitOfWork` when an operation changes state.

## DDD Guidance

The starter is DDD-inspired, not domain-filled. Add aggregates only when a real business capability exists. Keep invariants inside entities/value objects and expose them through explicit methods. Use domain events for meaningful business facts, not for technical notifications.

## API Conventions

- Routes: `/api/v{version:apiVersion}/[controller]`.
- Controllers are authorized by default through the fallback policy.
- Anonymous endpoints are explicitly allowed only for `/health/live`, `/health/ready`, and non-production OpenAPI.
- Expected application errors return `ProblemDetails`.
- Validation failures return `ValidationProblemDetails`.

## Database

EF Core migrations are reviewed and converted to idempotent SQL scripts in CI. The app must not call `Database.Migrate()` on startup.
