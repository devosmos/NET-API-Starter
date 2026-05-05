# Runbook

## Health Checks

- Liveness: `/health/live`
- Readiness: `/health/ready`

Liveness should stay independent from external dependencies. Readiness includes database connectivity.

## Common Incidents

### API returns 401

Check:

- `Authentication:Authority`
- `Authentication:Audience`
- Entra app registration exposed API/audience
- Token issuer and audience claims

### API returns 500 on data access

Check:

- App Service managed identity has a contained database user.
- Key Vault reference for `ConnectionStrings--Default` resolves.
- VNet integration is enabled.
- SQL private endpoint DNS resolves from the app.

### Deployment slot smoke test fails

Check:

- Staging slot configuration.
- Key Vault access for the slot managed identity.
- App Service runtime stack.
- Application Insights logs.

## Database Access Setup

After infrastructure deployment, create a contained database user for the App Service managed identity from a network-authorized SQL session:

```sql
CREATE USER [<web-app-name>] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [<web-app-name>];
ALTER ROLE db_datawriter ADD MEMBER [<web-app-name>];
ALTER ROLE db_ddladmin ADD MEMBER [<web-app-name>];
```

For least privilege, replace broad roles with migration-time and runtime-specific permissions once real features exist.
