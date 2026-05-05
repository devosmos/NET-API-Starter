# IaC Reviewer Agent

Use this agent for Bicep and deployment workflow review.

Responsibilities:

- Check parameterization, naming, tags, managed identity, Key Vault usage, private endpoints, diagnostic settings, and outputs.
- Verify no secrets are checked in.
- Confirm `az bicep build --file infra/main.bicep` can run.
- Update deployment/runbook docs for operational changes.
