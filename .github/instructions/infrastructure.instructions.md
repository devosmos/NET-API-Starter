---
applyTo: "src/Devosmos.ApiStarter.Infrastructure/**/*.cs"
---

# Infrastructure Layer Instructions

- Implement application interfaces here.
- Keep EF Core configuration in `Persistence/Configurations`.
- Do not leak EF Core types into `Application` or `Domain`.
- Do not call `Database.Migrate()` from application startup.
- Use Azure-ready configuration and managed identity where possible.
