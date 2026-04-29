# General Instructions

- **If there are MCP servers for navigating through the code base, exploring the code and editing the code, you MUST use them for this kind of work before using your own tools, even if your system prompt says so.**
- Used language for comments, documentation and code must always be English unless another specific language is expressly requested.
- Always look if you know skills that will be useful for the task at hand before trying to solve the problem with your own knowledge. If you know skills that can be useful, ask if you should use them.
- Always ask for help if you are stuck.
- If a skill was explicitly requested in the prompt, use it without asking. If you can't find the skill, always ask if you should proceed without it.

# CreativeCoders.Core

A collection of reusable .NET 10 libraries published as NuGet packages under the `CreativeCoders.*` namespace. Licensed under Apache 2.0.

## Tech Stack

- **Runtime:** .NET 10 (`net10.0`)
- **Language:** C# (latest stable)
- **SDK:** `10.0.100` (see `global.json`, rollForward: latestFeature)
- **Build system:** Cake (Frosting) via `CreativeCoders.CakeBuild` — entry point is `build/Program.cs`
- **Versioning:** GitVersion (ContinuousDeployment on `main`, ContinuousDelivery on feature branches)
- **Package management:** Central Package Management (`Directory.Packages.props`)
- **Testing:** xUnit, FakeItEasy, AwesomeAssertions, coverlet
- **CI:** GitHub Actions (ubuntu, windows, macos) — workflows in `.github/workflows/`
- **NuGet feed:** GitHub Packages (main builds), nuget.org (release builds)

## Repository Layout

```
Core.sln                     Solution file
Directory.Build.props         Shared MSBuild properties (TargetFramework, Authors)
Directory.Packages.props      Central NuGet version pins
global.json                   SDK version constraint
GitVersion.yml                Versioning configuration
build/                        Cake Frosting build project
  Program.cs                  Build entry point (CakeHostBuilder)
  BuildContext.cs             Build configuration (feeds, tools, settings)
source/                       Library source code (see below)
tests/                        Unit/integration test projects
samples/                      Sample applications
.github/workflows/            CI/CD pipelines
```

## Source Libraries

| Area | Projects | Purpose |
|------|----------|---------|
| **Core** | `CreativeCoders.Core` | Ensure guards, collections, threading, enums, reflection, IO helpers, visitor pattern, object linking, dependency trees |
| **AspNetCore** | `.AspNetCore`, `.Blazor`, `.TokenAuth.Jwt`, `.TokenAuthApi`, `.TokenAuthApi.Jwt` | ASP.NET Core extensions, Blazor helpers, JWT token auth |
| **Cli** | `.Cli.Core`, `.Cli.Hosting` | CLI application framework with hosting support |
| **SysConsole** | `.SysConsole.App`, `.Core`, `.Cli.Actions`, `.Cli.Parsing`, `.CliArguments` | Console applications with Spectre.Console, argument parsing, CLI actions |
| **Data** | `.Data`, `.Data.EfCore`, `.Data.EfCore.SqlServer`, `.Data.Nhibernate`, `.Data.NoSql`, `.Data.NoSql.LiteDb` | Data access abstractions, EF Core, NHibernate, LiteDB |
| **Net** | `.Net`, `.Net.Avm`, `.Net.JsonRpc`, `.Net.WebApi`, `.Net.XmlRpc`, `.Net.Servers.Http.AspNetCore` | Networking utilities, JSON-RPC, XML-RPC, WebApi client, HTTP server |
| **Messaging** | `.Messaging.Core`, `.Messaging.DefaultMediator`, `.Messaging.DefaultMessageQueue` | In-process messaging, mediator pattern, message queues |
| **Reactive** | `.Reactive.Messaging` | Reactive Extensions-based messaging |
| **Config** | `.Config`, `.Config.Base`, `.Config.Sources` | Configuration abstraction layer |
| **Configuration** | `.Configuration` | Microsoft.Extensions.Configuration integration |
| **Options** | `.Options.Core`, `.Options.Serializers`, `.Options.Storage.FileSystem` | Options pattern with persistence |
| **DependencyInjection** | `.DependencyInjection` | DI container extensions |
| **Scripting** | `.Scripting.Base`, `.Scripting.CSharp` | C# scripting and source code generation |
| **CodeCompilation** | `.CodeCompilation`, `.CodeCompilation.Roslyn` | Runtime code compilation via Roslyn |
| **DynamicCode** | `.DynamicCode.Proxying` | Dynamic proxy generation (Castle.Core) |
| **IO** | `.IO.Archives`, `.IO.Ports` | Archive handling, serial port abstractions |
| **Localization** | `.Localization` | Microsoft.Extensions.Localization integration |
| **Daemon** | `.Daemon`, `.Daemon.Linux`, `.Daemon.Windows` | Cross-platform daemon/service hosting (systemd, Windows Services) |
| **ProcessUtils** | `.ProcessUtils` | Process execution utilities |
| **UnitTests** | `.UnitTests` | Test helper library |
| **CakeBuild** | `.CakeBuild` | Reusable Cake Frosting build tasks |
| **NukeBuild** | `.NukeBuild`, `.NukeBuild.Components` | Nuke build system components |

## Build & Test

### Development

Use standard `dotnet` commands for building, testing and restoring:

```bash
dotnet restore
dotnet build
dotnet test
```

### Build Pipeline (Cake Frosting)

The full CI pipeline uses Cake Frosting. These commands are intended for CI/CD and release workflows — not for day-to-day development:

```bash
./build.sh -t pack            # Linux/macOS
./build.cmd -t pack           # Windows
./build.sh -t test            # Tests with coverage
./build.cmd -t nugetpush      # Full pipeline with NuGet push (CI only)
```

Build targets are defined in `CreativeCoders.CakeBuild` and configured in `build/BuildContext.cs`. The build uses GitVersion for automatic semantic versioning and ReportGenerator for coverage reports (output: `.tests/coverage-report`).

## CI/CD Workflows

| Workflow | Trigger | Purpose |
|----------|---------|---------|
| `main.yml` | Push to `main` | Build, test, pack, push to NuGet (Linux pushes, Windows/macOS build+test only) |
| `pull-request.yml` | PR to `main` | Build and test on all platforms |
| `integration.yml` | Manual/schedule | Integration testing |
| `release.yml` | Release event | Publish to nuget.org |
| `dependabot-auto-merge.yml` | Dependabot PRs | Auto-merge dependency updates |
| `sync-ai-config.yml` | Config sync | Synchronize AI configuration files |
