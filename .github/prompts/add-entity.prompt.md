# Add Entity Prompt

Create or update a domain model for a real business concept.

Requirements:

- Put domain types under `src/Devosmos.ApiStarter.Domain/<Feature>`.
- Use aggregate roots only when consistency boundaries are clear.
- Encode invariants in constructors, factories, value objects, and methods.
- Raise domain events for meaningful business facts.
- Avoid public setters for invariant-protected state.
- Add unit tests for invariants and value objects.

Do not add persistence concerns to the domain.
