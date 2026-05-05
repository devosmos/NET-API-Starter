# API Feature Skill

Use this skill when adding a real business feature to `Devosmos.ApiStarter`.

## Workflow

1. Confirm the business capability, route, auth requirements, and success/error outcomes.
2. Add or update domain types only when a real invariant exists.
3. Add application request, response, validator, and handler.
4. Add infrastructure persistence/configuration only if state is required.
5. Add a thin controller endpoint.
6. Add unit tests for domain/application behavior.
7. Add integration tests for HTTP, auth, validation, expected errors, and persistence.
8. Run build, tests, format, and migration checks.

## Guardrails

- Do not create fake sample domains.
- Do not put business logic in controllers.
- Do not reference EF Core from `Application` or `Domain`.
- Use `Result<T>` for expected failures.
- Use FluentValidation for input validation.
