# ADR 0002: Reviewed SQL Migrations

## Status

Accepted

## Decision

Do not run EF Core migrations automatically on API startup. CI generates idempotent SQL migration scripts for review and controlled execution.

## Consequences

- Production startup is safer and predictable.
- Database changes require an operational handoff.
- Private Azure SQL deployments need a network-authorized script executor.
