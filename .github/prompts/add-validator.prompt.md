# Add Validator Prompt

Add FluentValidation validation for an operation request.

Requirements:

- Place the validator next to the request in `Application`.
- Validate transport/input shape, not deep domain invariants.
- Use clear messages and property names.
- Add unit tests through `IOperationExecutor` or validator-specific tests.
- Confirm invalid requests map to `ValidationProblemDetails`.
