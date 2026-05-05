# Devosmos API Starter

Production-oriented ASP.NET Core 10 API starter for Clean Architecture, DDD-inspired feature work, Azure App Service Linux, Azure SQL, Bicep, GitHub Actions OIDC, and AI-assisted development.

The starter is intentionally domain-neutral. It contains platform conventions and one system metadata endpoint, but no sample products, orders, todos, invoices, or fake aggregates.

## Quickstart

1. Install .NET 10 SDK, Docker, and Azure CLI.
2. Start local SQL Server:

```powershell
docker compose up -d sql
```

3. Restore, build, and test:

```powershell
dotnet tool restore
dotnet restore Devosmos.ApiStarter.sln
dotnet build Devosmos.ApiStarter.sln --configuration Release
dotnet test Devosmos.ApiStarter.sln --configuration Release --collect:"XPlat Code Coverage"
```

4. Run the API:

```powershell
dotnet run --project src/Devosmos.ApiStarter.Api
```

Local OpenAPI is exposed at `/openapi/v1.json`. Production does not expose OpenAPI unless `OpenApi:Enabled` is explicitly enabled and the environment is not `Production`.

## Architecture

- `Domain`: entities, value objects, domain errors, events, aggregate base types. Depends on nothing.
- `Application`: operations, DTOs, validators, result/error model, application interfaces. Depends on `Domain`.
- `Infrastructure`: EF Core, Azure SQL, repositories, unit of work, external implementations. Depends on `Application` and `Domain`.
- `Api`: controllers, auth, routing, health, OpenAPI, security headers, rate limiting, composition root.

Routes use `/api/v{version:apiVersion}/[controller]`. Controllers are authorized by default through the fallback policy. `/health/live` and `/health/ready` are anonymous.

## Configuration

Important keys:

- `Authentication:Authority`
- `Authentication:Audience`
- `ConnectionStrings:Default`
- `Cors:AllowedOrigins`
- `OpenApi:Enabled`
- `RateLimiting:PermitLimit`
- `RateLimiting:WindowSeconds`
- `Observability:*`

Use `src/Devosmos.ApiStarter.Api/appsettings.Local.example.json` as a safe local template. Do not commit `appsettings.Local.json`.

## Database Migrations

Production database migrations are not applied on application startup. Generate reviewed idempotent SQL:

```powershell
dotnet ef migrations script --idempotent --project src/Devosmos.ApiStarter.Infrastructure --startup-project src/Devosmos.ApiStarter.Api --context AppDbContext --output artifacts/sql/Devosmos.ApiStarter.sql
```

Apply scripts through a network-authorized operator, DBA, or runner inside the VNet when Azure SQL uses a private endpoint.

## AI-Assisted Development

Use the assets in `.github/copilot-instructions.md`, `.github/instructions`, `.github/prompts`, `.github/agents`, `.github/skills`, and `AGENTS.md` to add real business slices later. New features should be generated as vertical use-case operations while preserving Clean Architecture dependencies.
