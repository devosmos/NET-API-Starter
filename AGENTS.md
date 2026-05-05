# Agent Guide

This repository is a production API starter. Treat it as infrastructure for future business slices, not as a sample app.

## Non-Negotiables

- Preserve Clean Architecture dependencies:
  - `Domain` depends on nothing.
  - `Application` depends on `Domain`.
  - `Infrastructure` depends on `Application` and `Domain`.
  - `Api` depends on `Application` and composes `Infrastructure`.
- Do not add fake business domains such as orders, todos, invoices, products, or customers.
- Add real features as vertical slices: controller endpoint, operation request/response, validator, handler, domain model, persistence config, tests, and docs.
- Do not apply EF Core migrations automatically at application startup.
- Keep controllers thin. Put use-case behavior in `Application`.
- Return expected failures through `Result<T>` and consistent `ProblemDetails`.
- Keep secrets out of source control.

## Verification

Run these before proposing a change as complete:

```powershell
dotnet restore Devosmos.ApiStarter.sln
dotnet build Devosmos.ApiStarter.sln --configuration Release
dotnet test Devosmos.ApiStarter.sln --configuration Release --collect:"XPlat Code Coverage"
dotnet format Devosmos.ApiStarter.sln --verify-no-changes
az bicep build --file infra/main.bicep
```

For model changes, also run:

```powershell
dotnet ef migrations has-pending-model-changes --project src/Devosmos.ApiStarter.Infrastructure --startup-project src/Devosmos.ApiStarter.Api --context AppDbContext
```
