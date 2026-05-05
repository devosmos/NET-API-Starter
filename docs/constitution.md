# Constitution

## Principles

1. Keep the starter domain-neutral until a real business capability is requested.
2. Prefer clear vertical slices over generic abstractions.
3. Keep dependencies flowing inward.
4. Make invalid states hard to represent in the domain.
5. Return expected failures explicitly.
6. Keep infrastructure replaceable from the application boundary.
7. Treat production database changes as reviewed operational events.
8. Keep AI-generated changes bound to the same engineering standards as human changes.

## Quality Gates

- Build and tests pass.
- Formatting is verified.
- New behavior has focused unit or integration coverage.
- API behavior has explicit response contracts.
- Infrastructure changes build through Bicep.
- Security-sensitive configuration is parameterized or secret-backed.

## Forbidden Shortcuts

- Sample business aggregates.
- Controllers containing business logic.
- Infrastructure dependencies inside `Domain` or `Application`.
- Automatic production migrations on app startup.
- Checked-in secrets.
- Public production OpenAPI by default.
