---
applyTo: "tests/**/*.cs"
---

# Test Instructions

- Use xUnit v3.
- Use FluentAssertions 7.2.x for assertions.
- Use NSubstitute for mocks in unit tests.
- Use `Microsoft.AspNetCore.Mvc.Testing` for API tests.
- Use Testcontainers.MsSql and Respawn for SQL Server integration tests.
- Use `TestContext.Current.CancellationToken` when calling async APIs that accept cancellation tokens.
- Keep test names behavior-focused.
