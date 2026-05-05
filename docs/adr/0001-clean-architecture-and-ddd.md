# ADR 0001: Clean Architecture And DDD-Inspired Modeling

## Status

Accepted

## Decision

Use four projects: `Domain`, `Application`, `Infrastructure`, and `Api`. Keep dependencies flowing inward and model business rules in the domain only when real business behavior is introduced.

## Consequences

- Controllers stay thin.
- Application operations are easy to test.
- Infrastructure can change without leaking into business rules.
- Feature additions require a few coordinated files, so prompts and skills guide the workflow.
