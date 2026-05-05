# Bicep IaC Skill

Use this skill for Azure infrastructure changes.

## Workflow

1. Update `infra/main.bicep`.
2. Update `infra/environments/dev.bicepparam` and `prod.bicepparam`.
3. Prefer managed identity and Key Vault references.
4. Keep production data services private where possible.
5. Add or adjust deployment outputs for GitHub Actions.
6. Validate:

```powershell
az bicep build --file infra/main.bicep
az deployment group what-if --resource-group <rg> --template-file infra/main.bicep --parameters infra/environments/dev.bicepparam
```

7. Update deployment and runbook docs.

## Guardrails

- Do not commit secrets.
- Keep environment-specific values in parameters.
- Use tags consistently.
- Include diagnostics for production-facing services.
