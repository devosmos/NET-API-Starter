# ADR 0003: Azure App Service And Bicep

## Status

Accepted

## Decision

Use Azure App Service Linux, Azure SQL, Key Vault, Application Insights, Log Analytics, VNet integration, and SQL private endpoint provisioned through Bicep.

## Consequences

- The baseline fits common enterprise Azure operations.
- Deployments are repeatable and reviewable.
- The starter avoids Kubernetes complexity until a real scaling or platform requirement appears.
