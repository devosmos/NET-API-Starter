# Add EF Core Migration Prompt

Create an EF Core migration for a real model change.

Requirements:

- Ensure the model is configured in `Infrastructure/Persistence/Configurations`.
- Run:

```powershell
dotnet ef migrations add <Name> --project src/Devosmos.ApiStarter.Infrastructure --startup-project src/Devosmos.ApiStarter.Api --context AppDbContext --output-dir Persistence/Migrations
dotnet ef migrations script --idempotent --project src/Devosmos.ApiStarter.Infrastructure --startup-project src/Devosmos.ApiStarter.Api --context AppDbContext --output artifacts/sql/<Name>.sql
dotnet ef migrations has-pending-model-changes --project src/Devosmos.ApiStarter.Infrastructure --startup-project src/Devosmos.ApiStarter.Api --context AppDbContext
```

- Do not apply migrations on startup.
- Add reviewer notes for destructive operations.
