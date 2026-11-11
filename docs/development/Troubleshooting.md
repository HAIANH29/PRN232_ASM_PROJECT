# Troubleshooting

Common problems when running the platform locally.

## Docker

- **Port already in use**: see `Service-Port-Map.md` and change the host-side port.
- **Database not ready**: services wait on database health checks. Run `docker compose ps` and check the `identity-db`, `diet-knowledge-db`, etc. containers are `healthy`.
- **Stale schema or seed data**: reset volumes with `docker compose down -v`, then `docker compose up -d --build`.
- **`.env` missing**: run `copy .env.example .env` (PowerShell) or `cp .env.example .env`.

## Build

- **SDK version error**: the repo pins the SDK in `global.json`. Install the matching .NET SDK.
- **Restore fails in a sandbox**: set `DOTNET_CLI_HOME` to a writable folder, for example `$env:DOTNET_CLI_HOME = "$PWD\.dotnet"`.

## Auth

- **401 from the gateway**: log in again; JWTs are signed with `JWT_SIGNING_KEY`, so changing it invalidates old tokens.
- **Default demo accounts** are defined in `.env.example`.

## Messaging and AI

- **No recommendations or emails**: check RabbitMQ at `http://localhost:15672` and the worker logs with `docker compose logs recommendation-service notification-worker`.
- **Gemini calls skipped**: `GEMINI_ENABLED=false` by default, so a safe fallback is used. Set a real key and enable it to use Gemini.
- **Semantic search returns nothing**: ensure `KNOWLEDGE_INDEX_ON_STARTUP=true` and that approved content exists.

## Tests

```powershell
dotnet test LongevityDietPlatform.sln
```
