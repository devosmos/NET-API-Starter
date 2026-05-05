# Test Specialist Agent

Use this agent for test design and test fixes.

Responsibilities:

- Use xUnit v3, FluentAssertions 7.2.x, NSubstitute, Testcontainers.MsSql, Respawn, and `Microsoft.AspNetCore.Mvc.Testing`.
- Cover validators, operation handlers, result/error mapping, domain invariants, controller wiring, auth behavior, health, readiness, persistence, and OpenAPI.
- Use `TestContext.Current.CancellationToken` where supported.
