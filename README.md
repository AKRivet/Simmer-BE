# Simmer Backend

REST API service built with ASP.NET Core (C#) on .NET 10 LTS.

## Tech Stack

| Layer | Choice |
|---|---|
| Runtime | .NET 10 LTS |
| Framework | ASP.NET Core Web API |
| OpenAPI | `Microsoft.AspNetCore.OpenApi` 10.0.12 + `Microsoft.Extensions.ApiDescription.Server` 10.0.12 |
| Tests | xunit 2.9.3 + `Microsoft.AspNetCore.Mvc.Testing` |
| Coverage | coverlet.collector |

## Project layout

```
src/
  SimmerBackend.Api/          — API host (controllers, models, config)
    Controllers/              — MVC API controllers
    Properties/               — launchSettings.json
    Program.cs                — application entry-point and DI wiring
    appsettings*.json         — runtime configuration
tests/
  SimmerBackend.Api.Tests/    — xunit integration tests via WebApplicationFactory
SimmerBackend.sln
Directory.Build.props         — shared MSBuild defaults (nullable, warnings-as-errors)
global.json                   — SDK version pin
.editorconfig                 — code-style rules
```

## Prerequisites

Install the .NET 10 LTS SDK (no root required):

```bash
curl -sSL https://dot.net/v1/dotnet-install.sh | bash -s -- --channel LTS
export PATH="$HOME/.dotnet:$PATH"
```

## Build

```bash
export PATH="$HOME/.dotnet:$PATH"
dotnet build
```

## Run

```bash
export PATH="$HOME/.dotnet:$PATH"
dotnet run --project src/SimmerBackend.Api
# API:      http://localhost:5000
# OpenAPI:  http://localhost:5000/openapi/v1.json
```

## Test

```bash
export PATH="$HOME/.dotnet:$PATH"
dotnet test
```

## OpenAPI document

The machine-readable API description lives at `.polaira/openapi.json` (gitignored — it is a build artefact).
Regenerate it with:

```bash
bash .polaira/emit-openapi.sh
```

The script builds the project in Release mode; `Microsoft.Extensions.ApiDescription.Server` runs the
app in document-generation mode and writes the OpenAPI 3.1 JSON without starting a server.

## Endpoints

| Method | Path | Description |
|---|---|---|
| `GET` | `/health` | Liveness probe — returns `{ "status": "healthy", "timestamp": "..." }` |
| `GET` | `/openapi/v1.json` | OpenAPI 3.1 document (Development environment only) |

## Security audit

Dependency vulnerability audit run against NuGet during initial scaffold (2026-10-01):

```
The given project `SimmerBackend.Api` has no vulnerable packages given the current sources.
```

> Note: `Microsoft.NET.Test.Sdk` 18.10.1 ships satellite assemblies tagged with locale codes that
> .NET 10 does not recognise (`NETSDK1188`). This is a cosmetic tooling warning, not a security
> finding; it is suppressed via `MSBuildWarningsAsMessages` in `Directory.Build.props`.
