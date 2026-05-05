# EF Core Migration Skill

Use this skill for database model and migration changes.

## Workflow

1. Update EF Core configuration in `Infrastructure/Persistence/Configurations`.
2. Keep domain state protected and mapped explicitly.
3. Generate a migration:

```powershell
dotnet ef migrations add <Name> --project src/Devosmos.ApiStarter.Infrastructure --startup-project src/Devosmos.ApiStarter.Api --context AppDbContext --output-dir Persistence/Migrations
```

4. Generate reviewed SQL:

```powershell
dotnet ef migrations script --idempotent --project src/Devosmos.ApiStarter.Infrastructure --startup-project src/Devosmos.ApiStarter.Api --context AppDbContext --output artifacts/sql/<Name>.sql
```

5. Check for pending model changes:

```powershell
dotnet ef migrations has-pending-model-changes --project src/Devosmos.ApiStarter.Infrastructure --startup-project src/Devosmos.ApiStarter.Api --context AppDbContext
```

## Guardrails

- Never call `Database.Migrate()` during app startup.
- Flag destructive schema changes in PR notes.
- Production scripts must be applied by a network-authorized operator or runner.
