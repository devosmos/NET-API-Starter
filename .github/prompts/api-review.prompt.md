# API Review Prompt

Review the API change for production readiness.

Check:

- Clean Architecture dependency direction.
- Authorization defaults and anonymous endpoints.
- Route/versioning conventions.
- Validation and `ProblemDetails` consistency.
- Operation handler behavior and error mapping.
- EF Core migration safety.
- Unit and integration test coverage.
- Bicep/workflow updates when deployment shape changes.

Report findings by severity with file paths and line numbers.
