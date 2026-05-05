---
applyTo: "infra/**/*.bicep"
---

# Bicep Instructions

- Keep modules and resources parameterized by environment.
- Do not check in secrets.
- Prefer managed identity and Key Vault references.
- Use private endpoints for data services when production-ready.
- Add outputs needed by deployment workflows.
- Validate with `az bicep build --file infra/main.bicep`.
