---
applyTo: "src/Devosmos.ApiStarter.Domain/**/*.cs"
---

# Domain Layer Instructions

- Do not reference application, infrastructure, ASP.NET Core, EF Core, or external services.
- Put invariants inside entities, aggregate roots, and value objects.
- Use domain errors/exceptions for invariant violations.
- Use domain events for business facts.
- Avoid anemic setters; expose meaningful methods.
