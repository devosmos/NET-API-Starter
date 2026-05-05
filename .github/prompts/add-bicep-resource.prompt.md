# Add Bicep Resource Prompt

Add or modify Azure infrastructure through Bicep.

Requirements:

- Keep environment differences in parameter files.
- Use managed identity and Key Vault references where possible.
- Add diagnostic settings for production services when practical.
- Update deployment outputs if workflows need new values.
- Run `az bicep build --file infra/main.bicep`.
- Update `docs/deployment.md` and `docs/runbook.md` for operational changes.
