# Contributing

## Branches

- `main`: stable, demo-ready.
- `feature/<short-name>`: one feature or fix per branch.
- `docs/<short-name>`: documentation-only changes.

## Commit messages

Use a short prefix, then a clear summary:

- `feat:` new behavior
- `fix:` bug fix
- `docs:` documentation
- `test:` tests
- `refactor:` no behavior change
- `chore:` tooling, config

Example: `docs: add service port map`

## Before opening a pull request

1. `dotnet build LongevityDietPlatform.sln`
2. `dotnet test LongevityDietPlatform.sln`
3. Update `PROJECT_PROGRESS.md` if the change completes a tracked item.
4. Never commit real secrets. Use `.env.example` for placeholders only.
