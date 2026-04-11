# CreativeCoders.CakeBuild

A reusable build automation framework built on [Cake Frosting](https://cakebuild.net/docs/running-builds/runners/cake-frosting) for .NET projects. Provides a fluent builder API with pre-built CI/CD tasks — clean, build, test, pack, publish, create GitHub releases, and more — so you can set up a complete build pipeline with minimal code.

## Features

- 🏗️ **Fluent Builder API** — Configure your build pipeline with `CakeHostBuilder` in just a few lines
- 📦 **Pre-built Tasks** — Standard CI/CD tasks out of the box: Clean, Restore, Build, Test, Pack, Publish, NuGet Push, Code Coverage, GitHub Releases, Distribution Packages
- ⚙️ **Settings Interfaces** — Customize task behavior by implementing strongly-typed settings interfaces
- 🔍 **Auto-Discovery** — Automatically finds Git root, solution files, and test projects
- 🏷️ **GitVersion Integration** — Semantic versioning via GitVersion with static fallback
- 🐙 **GitHub Actions Support** — Log grouping and build server integration

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) or later

### Setup

Create a new console application and reference the `CreativeCoders.CakeBuild` package:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ProjectReference Include="CreativeCoders.CakeBuild" Version="LATEST" />
</Project>
```

### Minimal Example

```csharp
using CreativeCoders.CakeBuild;

CakeHostBuilder.Create()
    .UseBuildContext<MyBuildContext>()
    .AddDefaultTasks()
    .AddBuildServerIntegration()
    .InstallTools(
        new DotNetToolInstallation("GitVersion.Tool", "6.5.1"),
        new DotNetToolInstallation("dotnet-reportgenerator-globaltool", "5.5.1"))
    .Build()
    .Run(args);
```

## Usage

### Custom Build Context

Extend `CakeBuildContext` and implement the settings interfaces for the tasks you want to configure:

```csharp
public class MyBuildContext(ICakeContext context) : CakeBuildContext(context),
    IDefaultTaskSettings,
    ICreateDistPackagesTaskSettings
{
    public string Copyright => $"{DateTime.Now.Year} My Company";

    public string PackageProjectUrl => "https://github.com/my-org/my-repo";

    public string PackageLicenseExpression => PackageLicenseExpressions.Apache20;

    public string NuGetFeedUrl => "https://api.nuget.org/v3/index.json";

    public IEnumerable<DistPackage> DistPackages =>
    [
        new("my-app-linux-x64", "artifacts/publish/my-app/linux-x64", DistPackageFormat.TarGz),
        new("my-app-win-x64", "artifacts/publish/my-app/win-x64", DistPackageFormat.Zip)
    ];
}
```

### Available Tasks

All default tasks are registered via `AddDefaultTasks()` and execute in dependency order:

| Task | Description | Depends On |
|------|-------------|------------|
| **Clean** | Removes `bin/`, `obj/`, and artifact directories | — |
| **Restore** | Restores NuGet packages | Clean |
| **Build** | Builds the solution with version info from GitVersion | Restore |
| **Test** | Runs tests with optional code coverage collection | Build |
| **CodeCoverage** | Generates coverage reports via ReportGenerator | Test |
| **Pack** | Creates NuGet packages with metadata | Build |
| **NuGetPush** | Pushes packages to a NuGet feed | Pack |
| **Publish** | Publishes applications to output directories | Build |
| **CreateDistPackages** | Creates `.tar.gz` / `.zip` distribution archives | Publish |
| **CreateGitHubRelease** | Creates a GitHub release with assets via Octokit | — |

### Settings Interfaces

Each task reads its configuration from a settings interface. Implement only the ones you need:

| Interface | Configures |
|-----------|------------|
| `ICleanTaskSettings` | Directories to clean |
| `ITestTaskSettings` | Test projects, coverage options |
| `ICodeCoverageTaskSettings` | Report types and file patterns |
| `IPackTaskSettings` | Package output, metadata (URL, license, copyright) |
| `INuGetPushTaskSettings` | Feed URL, API key, skip flag |
| `IPublishTaskSettings` | Per-project publish configuration (runtime, self-contained) |
| `ICreateDistPackagesTaskSettings` | Distribution package definitions and output path |
| `ICreateGitHubReleaseTaskSettings` | Release metadata, assets, GitHub token |

> [!TIP]
> Implement `IDefaultTaskSettings` to get all standard settings interfaces in one go.

### Tool Installation

Register external tools via the builder:

```csharp
CakeHostBuilder.Create()
    .InstallTools(
        new DotNetToolInstallation("GitVersion.Tool", "6.5.1"),
        new NuGetToolInstallation("ReportGenerator", "5.5.1"))
    // ...
```

### GitHub Actions Integration

Enable log grouping for GitHub Actions:

```csharp
CakeHostBuilder.Create()
    .AddBuildServerIntegration()
    // ...
```

This registers task setup/teardown hooks that create collapsible log groups in GitHub Actions.

### Generic Task Templates

For advanced scenarios, use the generic task templates (`BuildTask<T>`, `TestTask<T>`, etc.) with a custom context type instead of the default `CakeBuildContext`:

```csharp
[TaskName("Build")]
[IsDependentOn(typeof(RestoreTask<MyContext>))]
public class MyBuildTask : BuildTask<MyContext> { }
```

## Sample

See [`samples/CakeBuildSample`](../../../samples/CakeBuildSample) for a complete working example that demonstrates the full pipeline setup with custom context, publishing, and distribution package creation.
