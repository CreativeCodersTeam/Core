# Plan: Aufteilung des Core-Monorepos in separate Repositories

## Übersicht

Das aktuelle `Core`-Repository enthält ~51 Source-Projekte und ~21 Test-Projekte in einem einzigen Monorepo.
Ziel ist die Aufteilung in 8 eigenständige GitHub-Repositories mit klaren Verantwortlichkeiten.
Abhängigkeiten zwischen den Repos werden über NuGet-Pakete abgebildet.
Dependabot übernimmt automatische Updates für Minor- und Patch-Versionen.

---

## Ziel-Repositories

| Repo-Name            | Beschreibung                                  |
|----------------------|-----------------------------------------------|
| `dotnet-libs-core`   | Fundament – Core, Localization, Build-Tools   |
| `dotnet-libs-cli`    | CLI-Framework und Konsolen-Utilities          |
| `dotnet-libs-config` | Konfiguration und Options                     |
| `dotnet-libs-messaging` | Messaging und Reactive                     |
| `dotnet-libs-data`   | Datenzugriff (EF Core, NHibernate, NoSQL)     |
| `dotnet-libs-net`    | Netzwerk und ASP.NET Core                     |
| `dotnet-libs-daemon` | Daemon / Windows-Service-Hosting              |
| `dotnet-libs-dyncode`| Dynamic Code, Scripting, Code Compilation     |
| `dotnet-libs-io`     | IO-Utilities (Archives, Ports)                |
| `dotnet-libs-update-orchestrator` | Zentrales Major-Update-Management  |

---

## Abhängigkeiten zwischen den Repos

Die folgende Tabelle zeigt, welche NuGet-Pakete jedes neue Repo aus den anderen konsumieren wird:

```
dotnet-libs-core        (keine Abhängigkeit zu anderen Repos)
dotnet-libs-io          → dotnet-libs-core
dotnet-libs-config      → dotnet-libs-core
dotnet-libs-messaging   → dotnet-libs-core
dotnet-libs-daemon      → dotnet-libs-core
dotnet-libs-dyncode     → dotnet-libs-core
dotnet-libs-cli         → dotnet-libs-core, dotnet-libs-config (ProcessUtils)
dotnet-libs-data        → dotnet-libs-core, dotnet-libs-config (EfCore.SqlServer braucht Config.Base)
dotnet-libs-net         → dotnet-libs-core, dotnet-libs-dyncode (Net.JsonRpc, Net.WebApi, Net.XmlRpc)
```

---

## Phase 1: Vorbereitung (im bestehenden Core-Repo)

### 1.1 Einführung von `Directory.Packages.props`

Aktuell sind NuGet-Versionen in jedem `.csproj` einzeln definiert.
Vor der Aufteilung sollte Central Package Management (CPM) eingeführt werden,
damit alle Repos dasselbe Pattern verwenden und die Dependabot-Konfiguration einheitlich bleibt.

**Datei: `Directory.Packages.props`**
```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  <ItemGroup>
    <!-- Alle PackageVersion-Einträge hier -->
    <PackageVersion Include="JetBrains.Annotations" Version="2025.2.4" />
    <PackageVersion Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="10.0.3" />
    <!-- ... -->
  </ItemGroup>
</Project>
```

### 1.2 Refactoring: `SysConsole` → `Cli.ConsoleCore`

Die `SysConsole.*`-Projekte sollen in das `dotnet-libs-cli`-Repo überführt werden.
Im Rahmen der Migration ist ein Refactoring/Rewrite zu `Cli.ConsoleCore` vorgesehen.

**Umzubenennen / umzustrukturieren:**
- `CreativeCoders.SysConsole.Core` → `CreativeCoders.Cli.ConsoleCore`
- `CreativeCoders.SysConsole.App` → in `CreativeCoders.Cli.ConsoleCore` integrieren oder als `CreativeCoders.Cli.ConsoleApp`
- `CreativeCoders.SysConsole.Cli.Parsing` → `CreativeCoders.Cli.Parsing`
- `CreativeCoders.SysConsole.Cli.Actions` → `CreativeCoders.Cli.Actions`
- `CreativeCoders.SysConsole.CliArguments` → `CreativeCoders.Cli.Arguments`

### 1.3 Entscheidung: `DependencyInjection`-Projekt

`CreativeCoders.DependencyInjection` ist ein dünner Wrapper um
`Microsoft.Extensions.DependencyInjection.Abstractions`. Optionen:

- **Option A (empfohlen):** In `dotnet-libs-core` aufnehmen, da alle Repos es indirekt brauchen
- **Option B:** Als eigenständiges Mikro-Paket in `dotnet-libs-core` bündeln und direkt exportieren

**Entscheidung: Option A** – `CreativeCoders.DependencyInjection` bleibt in `dotnet-libs-core`.

### 1.4 Entscheidung: `ProcessUtils`

`CreativeCoders.ProcessUtils` hängt nur von `CreativeCoders.Core` ab.
Es kann entweder nach `dotnet-libs-core` oder `dotnet-libs-cli` gehören.

**Entscheidung:** → `dotnet-libs-cli` (ProcessUtils ist häufig in CLI-Anwendungen relevant)

---

## Phase 2: Repository-Struktur je neuem Repo

### 2.1 `dotnet-libs-core`

**GitHub-Repo:** `CreativeCoders/dotnet-libs-core`

**Enthaltene Projekte (source):**
- `CreativeCoders.Core`
- `CreativeCoders.DependencyInjection`
- `CreativeCoders.Localization`
- `CreativeCoders.CakeBuild` *(hängt auf CreativeCoders.IO.Archives → NuGet-Ref nach dotnet-libs-io)*
- `CreativeCoders.UnitTests` *(split: Core-Teil bleibt hier, Net-Teil → dotnet-libs-net)*

**Enthaltene Projekte (tests):**
- `CreativeCoders.Core.UnitTests` *(ohne Config/Options-Tests → die wandern zu dotnet-libs-config)*
- `CreativeCoders.DependencyInjection.UnitTests`
- `CreativeCoders.Localization.UnitTests`
- `CreativeCoders.NukeBuild.Tests`
- `CreativeCoders.CakeBuild.Tests`

**NuGet-Abhängigkeiten zu anderen Repos:**
- `CreativeCoders.IO.Archives` (für CakeBuild) → aus `dotnet-libs-io`

**Externe NuGet-Pakete:**
- `JetBrains.Annotations`
- `Microsoft.Extensions.DependencyInjection.Abstractions`
- `Microsoft.Extensions.Localization`
- `System.IO.Abstractions`
- `Cake.Frosting`, `MimeMapping`, `Octokit` (für CakeBuild)

---

### 2.2 `dotnet-libs-cli`

**GitHub-Repo:** `CreativeCoders/dotnet-libs-cli`

**Enthaltene Projekte (source):**
- `CreativeCoders.Cli.Core`
- `CreativeCoders.Cli.Hosting`
- `CreativeCoders.Cli.ConsoleCore` *(refactored aus SysConsole.Core)*
- `CreativeCoders.Cli.Parsing` *(refactored aus SysConsole.Cli.Parsing)*
- `CreativeCoders.Cli.Actions` *(refactored aus SysConsole.Cli.Actions)*
- `CreativeCoders.Cli.Arguments` *(refactored aus SysConsole.CliArguments)*
- `CreativeCoders.ProcessUtils`

**Enthaltene Projekte (tests):**
- `CreativeCoders.Cli.Tests`
- `CreativeCoders.SysConsole.UnitTests` *(umbenannt nach Refactoring)*
- `CreativeCoders.SysConsole.App.UnitTests` *(umbenannt)*
- `CreativeCoders.SysConsole.Cli.Parsing.UnitTests` *(umbenannt)*
- `CreativeCoders.SysConsole.Cli.Actions.UnitTests` *(umbenannt)*
- `CreativeCoders.SysConsole.CliArguments.UnitTests` *(umbenannt)*

**NuGet-Abhängigkeiten zu anderen Repos:**
- `CreativeCoders.Core` → aus `dotnet-libs-core`

**Externe NuGet-Pakete:**
- `Spectre.Console`
- `Microsoft.Extensions.Configuration`
- `Microsoft.Extensions.DependencyInjection`

---

### 2.3 `dotnet-libs-config`

**GitHub-Repo:** `CreativeCoders/dotnet-libs-config`

**Enthaltene Projekte (source):**
- `CreativeCoders.Config.Base`
- `CreativeCoders.Config`
- `CreativeCoders.Config.Sources`
- `CreativeCoders.Configuration`
- `CreativeCoders.Options.Core`
- `CreativeCoders.Options.Serializers`
- `CreativeCoders.Options.Storage.FileSystem`

**Enthaltene Projekte (tests):**
- Config/Options-Tests aus `CreativeCoders.Core.UnitTests` *(herausgelöst)*

**NuGet-Abhängigkeiten zu anderen Repos:**
- `CreativeCoders.Core` → aus `dotnet-libs-core`

**Externe NuGet-Pakete:**
- `Microsoft.Extensions.Configuration`
- `Microsoft.Extensions.Options`
- `Newtonsoft.Json`
- `YamlDotNet`

---

### 2.4 `dotnet-libs-messaging`

**GitHub-Repo:** `CreativeCoders/dotnet-libs-messaging`

**Enthaltene Projekte (source):**
- `CreativeCoders.Messaging.Core`
- `CreativeCoders.Messaging.DefaultMediator`
- `CreativeCoders.Messaging.DefaultMessageQueue`
- `CreativeCoders.Reactive.Messaging`

**Enthaltene Projekte (tests):**
- `CreativeCoders.Messaging.UnitTests`
- `CreativeCoders.Reactive.UnitTests`

**NuGet-Abhängigkeiten zu anderen Repos:**
- `CreativeCoders.Core` → aus `dotnet-libs-core`

**Externe NuGet-Pakete:**
- `System.Reactive`
- `JetBrains.Annotations`

---

### 2.5 `dotnet-libs-data`

**GitHub-Repo:** `CreativeCoders/dotnet-libs-data`

**Enthaltene Projekte (source):**
- `CreativeCoders.Data`
- `CreativeCoders.Data.EfCore`
- `CreativeCoders.Data.EfCore.SqlServer`
- `CreativeCoders.Data.Nhibernate`
- `CreativeCoders.Data.NoSql`
- `CreativeCoders.Data.NoSql.LiteDb`

**Enthaltene Projekte (tests):**
- `CreativeCoders.Data.NoSql.LiteDb.Tests`

**NuGet-Abhängigkeiten zu anderen Repos:**
- `CreativeCoders.Core` → aus `dotnet-libs-core`
- `CreativeCoders.Config.Base` → aus `dotnet-libs-config` *(für EfCore.SqlServer)*

**Externe NuGet-Pakete:**
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `NHibernate`
- `LiteDB`

---

### 2.6 `dotnet-libs-net`

**GitHub-Repo:** `CreativeCoders/dotnet-libs-net`

**Enthaltene Projekte (source):**
- `CreativeCoders.Net`
- `CreativeCoders.Net.Avm`
- `CreativeCoders.Net.JsonRpc`
- `CreativeCoders.Net.WebApi`
- `CreativeCoders.Net.XmlRpc`
- `CreativeCoders.Net.Servers.Http.AspNetCore`
- `CreativeCoders.AspNetCore`
- `CreativeCoders.AspNetCore.Blazor`
- `CreativeCoders.AspNetCore.TokenAuthApi`
- `CreativeCoders.AspNetCore.TokenAuthApi.Jwt`
- `CreativeCoders.AspNetCore.TokenAuth.Jwt`
- `CreativeCoders.UnitTests` *(Net-Teil: HttpClient-Mocking-Helpers)*

**Enthaltene Projekte (tests):**
- `CreativeCoders.Net.UnitTests`
- `CreativeCoders.AspNetCore.Tests`
- `CreativeCoders.AspNetCore.Blazor.UnitTests`

**NuGet-Abhängigkeiten zu anderen Repos:**
- `CreativeCoders.Core` → aus `dotnet-libs-core`
- `CreativeCoders.DynamicCode.Proxying` → aus `dotnet-libs-dyncode`

**Externe NuGet-Pakete:**
- `Microsoft.Extensions.Http`
- `Newtonsoft.Json`
- `Polly`
- `Castle.Core`
- `Microsoft.AspNetCore.Mvc.Core`
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Microsoft.AspNetCore.Components`

---

### 2.7 `dotnet-libs-daemon`

**GitHub-Repo:** `CreativeCoders/dotnet-libs-daemon`

**Enthaltene Projekte (source):**
- `CreativeCoders.Daemon`
- `CreativeCoders.Daemon.Linux`
- `CreativeCoders.Daemon.Windows`

**Enthaltene Projekte (tests):**
- *(aktuell keine dedizierten Tests – bei Migration anlegen)*

**NuGet-Abhängigkeiten zu anderen Repos:**
- `CreativeCoders.Core` → aus `dotnet-libs-core`

**Externe NuGet-Pakete:**
- `Microsoft.Extensions.Hosting`
- `Microsoft.Extensions.Hosting.Systemd`
- `Microsoft.Extensions.Hosting.WindowsServices`

---

### 2.8 `dotnet-libs-dyncode`

**GitHub-Repo:** `CreativeCoders/dotnet-libs-dyncode`

**Enthaltene Projekte (source):**
- `CreativeCoders.DynamicCode.Proxying`
- `CreativeCoders.CodeCompilation`
- `CreativeCoders.CodeCompilation.Roslyn`
- `CreativeCoders.Scripting.Base`
- `CreativeCoders.Scripting.CSharp`

**Enthaltene Projekte (tests):**
- `CreativeCoders.CodeCompilation.UnitTests`
- `CreativeCoders.Scripting.UnitTests`

**NuGet-Abhängigkeiten zu anderen Repos:**
- `CreativeCoders.Core` → aus `dotnet-libs-core`

**Externe NuGet-Pakete:**
- `Castle.Core`
- `Microsoft.CodeAnalysis.CSharp`

---

### 2.9 `dotnet-libs-io`

**GitHub-Repo:** `CreativeCoders/dotnet-libs-io`

**Enthaltene Projekte (source):**
- `CreativeCoders.IO.Archives`
- `CreativeCoders.IO.Ports`

**Enthaltene Projekte (tests):**
- `CreativeCoders.IO.UnitTests`

**NuGet-Abhängigkeiten zu anderen Repos:**
- `CreativeCoders.Core` → aus `dotnet-libs-core`

**Externe NuGet-Pakete:**
- `System.IO.Ports`
- `System.Reactive`

---

## Phase 3: Standardstruktur je Repository

Jedes neue Repository erhält dieselbe Grundstruktur:

```
dotnet-libs-xxx/
├── .github/
│   ├── workflows/
│   │   ├── pull-request.yml
│   │   ├── main.yml
│   │   └── release.yml
│   └── dependabot.yml
├── source/
│   └── <DomainOrdner>/
│       └── <Projektname>/
│           └── <Projektname>.csproj
├── tests/
│   └── <TestProjektname>/
│       └── <TestProjektname>.csproj
├── samples/           (optional, nur wenn relevant)
├── build/
│   └── Build.csproj  (NUKE)
├── Directory.Build.props
├── Directory.Packages.props
├── global.json
├── <RepoName>.sln
├── .editorconfig
├── .gitignore
└── README.md
```

---

## Phase 4: GitHub Actions Workflows

### 4.1 `pull-request.yml` (Vorlage für alle Repos)

```yaml
name: Pull Request

on:
  pull_request:
    branches: [ main ]

jobs:
  build:
    strategy:
      matrix:
        os: [ubuntu-latest, windows-latest, macos-latest]
    runs-on: ${{ matrix.os }}
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          global-json-file: global.json
      - name: Cache NuGet packages
        uses: actions/cache@v4
        with:
          path: ~/.nuget/packages
          key: ${{ runner.os }}-nuget-${{ hashFiles('global.json', '**/*.csproj', '**/Directory.Packages.props') }}
      - name: Build and Pack
        run: ./build.cmd -t pack
      - name: Upload coverage
        uses: actions/upload-artifact@v4
        with:
          name: coverage-${{ matrix.os }}
          path: '**/coverage*.xml'
```

### 4.2 `main.yml`

```yaml
name: Main

on:
  push:
    branches: [ main ]

jobs:
  publish:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          global-json-file: global.json
      - name: Cache NuGet packages
        uses: actions/cache@v4
        with:
          path: ~/.nuget/packages
          key: ${{ runner.os }}-nuget-${{ hashFiles('global.json', '**/*.csproj', '**/Directory.Packages.props') }}
      - name: Publish to NuGet (Integration Feed)
        run: ./build.cmd -t nugetpush
        env:
          NUGET_ORG_TOKEN: ${{ secrets.NUGET_ORG_TOKEN }}

  build-other-platforms:
    strategy:
      matrix:
        os: [windows-latest, macos-latest]
    runs-on: ${{ matrix.os }}
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          global-json-file: global.json
      - run: ./build.cmd -t pack
```

### 4.3 `release.yml`

```yaml
name: Release

on:
  push:
    tags:
      - 'v**'

jobs:
  release:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          global-json-file: global.json
      - name: Publish to NuGet.org
        run: ./build.cmd -t nugetpush
        env:
          NUGET_ORG_TOKEN: ${{ secrets.NUGET_ORG_TOKEN }}
      - name: Upload coverage
        uses: actions/upload-artifact@v4
        with:
          name: coverage
          path: '**/coverage*.xml'
```

---

## Phase 5: Dependabot-Konfiguration

### 5.1 Strategie

- **Minor- und Patch-Updates:** Automatisch per Auto-Merge zusammenführen
- **Major-Updates:** PR wird erstellt, aber **nicht** automatisch gemergt (manuelle Review erforderlich)
- Update-Intervall: `weekly` (schlägt neue Versionen wöchentlich vor)
- Auto-Merge wird über ein separates Workflow-File `auto-merge-dependabot.yml` realisiert

### 5.2 `.github/dependabot.yml` (identisch in allen Repos)

```yaml
version: 2
updates:
  # NuGet-Pakete
  - package-ecosystem: "nuget"
    directory: "/"
    schedule:
      interval: "weekly"
      day: "monday"
      time: "06:00"
      timezone: "Europe/Berlin"
    open-pull-requests-limit: 10
    groups:
      # Microsoft.Extensions.*-Pakete gemeinsam updaten
      microsoft-extensions:
        patterns:
          - "Microsoft.Extensions.*"
      # ASP.NET Core Pakete
      aspnetcore:
        patterns:
          - "Microsoft.AspNetCore.*"
      # Entity Framework Core Pakete
      efcore:
        patterns:
          - "Microsoft.EntityFrameworkCore*"
      # xUnit Test-Pakete
      xunit:
        patterns:
          - "xunit*"
          - "xunit.runner.*"
      # Test-Infrastruktur
      test-infra:
        patterns:
          - "Microsoft.NET.Test.Sdk"
          - "coverlet.collector"
          - "XunitXml.TestLogger"

  # GitHub Actions
  - package-ecosystem: "github-actions"
    directory: "/"
    schedule:
      interval: "weekly"
      day: "monday"
      time: "06:00"
      timezone: "Europe/Berlin"
    open-pull-requests-limit: 5
```

### 5.3 `.github/workflows/auto-merge-dependabot.yml`

Dieser Workflow merged Dependabot-PRs **automatisch**, wenn es sich um Minor- oder Patch-Updates handelt
und alle CI-Checks grün sind.

```yaml
name: Auto-Merge Dependabot PRs

on:
  pull_request:
    types: [opened, synchronize, reopened]

permissions:
  contents: write
  pull-requests: write

jobs:
  auto-merge:
    runs-on: ubuntu-latest
    # Nur für Dependabot-PRs ausführen
    if: github.actor == 'dependabot[bot]'
    steps:
      - name: Dependabot metadata
        id: metadata
        uses: dependabot/fetch-metadata@v2
        with:
          github-token: "${{ secrets.GITHUB_TOKEN }}"

      - name: Auto-Merge für Minor- und Patch-Updates
        # Nur mergen wenn kein Major-Update
        if: |
          steps.metadata.outputs.update-type == 'version-update:semver-minor' ||
          steps.metadata.outputs.update-type == 'version-update:semver-patch'
        run: gh pr merge --auto --squash "$PR_URL"
        env:
          PR_URL: ${{ github.event.pull_request.html_url }}
          GH_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

> **Hinweis:** Branch Protection Rules müssen so konfiguriert sein, dass Auto-Merge erst nach
> erfolgreichem CI-Durchlauf aktiviert wird. Dazu in den Repository-Settings unter
> **Branches → Branch protection rules → main** folgendes aktivieren:
> - ✅ Require status checks to pass before merging
> - ✅ Require branches to be up to date before merging
> - ✅ Allow auto-merge

---

## Phase 6: Zentrales Major-Update-Management

Bei einem Major-Update eines Upstream-Repos (z.B. `dotnet-libs-core` v1.x → v2.0) müssen alle
abhängigen Repos koordiniert aktualisiert werden. Da Dependabot diese PRs **nicht automatisch mergt**,
braucht es eine zentrale Steuerung über ein dedizierten **Orchestrator-Repo**.

### 6.1 Konzept: `dotnet-libs-update-orchestrator`

Ein separates Repository (`CreativeCoders/dotnet-libs-update-orchestrator`) dient als zentrales
Steuerungs-Repo für Major-Updates. Es enthält keine Source-Projekte, sondern ausschließlich
GitHub Actions Workflows, die per `workflow_dispatch` oder automatisch ausgelöst werden.

```
dotnet-libs-update-orchestrator/
├── .github/
│   └── workflows/
│       ├── trigger-major-update.yml      ← manueller Auslöser (workflow_dispatch)
│       ├── cascade-update.yml            ← Kaskadierung durch Abhängigkeitsgraph
│       └── status-dashboard.yml          ← Übersicht aller laufenden Update-PRs
├── config/
│   └── dependency-graph.yml              ← Maschinenlesbarer Abhängigkeitsgraph
└── README.md
```

---

### 6.2 Maschinenlesbarer Abhängigkeitsgraph

**`config/dependency-graph.yml`** – definiert die Update-Reihenfolge und welche Repos
bei einem Major-Update eines bestimmten Pakets betroffen sind:

```yaml
# Abhängigkeitsgraph für Major-Update-Kaskadierung
# "downstream" = diese Repos müssen nach einem Update des aktuellen Repos aktualisiert werden

repos:
  dotnet-libs-core:
    packages:
      - CreativeCoders.Core
      - CreativeCoders.DependencyInjection
      - CreativeCoders.Localization
      - CreativeCoders.CakeBuild
    downstream:
      - dotnet-libs-io
      - dotnet-libs-config
      - dotnet-libs-messaging
      - dotnet-libs-daemon
      - dotnet-libs-dyncode
      - dotnet-libs-cli
      - dotnet-libs-data
      - dotnet-libs-net

  dotnet-libs-io:
    packages:
      - CreativeCoders.IO.Archives
      - CreativeCoders.IO.Ports
    downstream:
      - dotnet-libs-core   # CakeBuild hängt von IO.Archives ab

  dotnet-libs-config:
    packages:
      - CreativeCoders.Config.Base
      - CreativeCoders.Config
      - CreativeCoders.Config.Sources
      - CreativeCoders.Configuration
      - CreativeCoders.Options.Core
      - CreativeCoders.Options.Serializers
      - CreativeCoders.Options.Storage.FileSystem
    downstream:
      - dotnet-libs-cli
      - dotnet-libs-data

  dotnet-libs-dyncode:
    packages:
      - CreativeCoders.DynamicCode.Proxying
      - CreativeCoders.CodeCompilation
      - CreativeCoders.CodeCompilation.Roslyn
      - CreativeCoders.Scripting.Base
      - CreativeCoders.Scripting.CSharp
    downstream:
      - dotnet-libs-net

  dotnet-libs-messaging:
    packages:
      - CreativeCoders.Messaging.Core
      - CreativeCoders.Messaging.DefaultMediator
      - CreativeCoders.Messaging.DefaultMessageQueue
      - CreativeCoders.Reactive.Messaging
    downstream: []

  dotnet-libs-daemon:
    packages:
      - CreativeCoders.Daemon
      - CreativeCoders.Daemon.Linux
      - CreativeCoders.Daemon.Windows
    downstream: []

  dotnet-libs-cli:
    packages:
      - CreativeCoders.Cli.Core
      - CreativeCoders.Cli.Hosting
      - CreativeCoders.Cli.ConsoleCore
      - CreativeCoders.Cli.Parsing
      - CreativeCoders.Cli.Actions
      - CreativeCoders.Cli.Arguments
      - CreativeCoders.ProcessUtils
    downstream: []

  dotnet-libs-data:
    packages:
      - CreativeCoders.Data
      - CreativeCoders.Data.EfCore
      - CreativeCoders.Data.EfCore.SqlServer
      - CreativeCoders.Data.Nhibernate
      - CreativeCoders.Data.NoSql
      - CreativeCoders.Data.NoSql.LiteDb
    downstream: []

  dotnet-libs-net:
    packages:
      - CreativeCoders.Net
      - CreativeCoders.Net.Avm
      - CreativeCoders.Net.JsonRpc
      - CreativeCoders.Net.WebApi
      - CreativeCoders.Net.XmlRpc
      - CreativeCoders.Net.Servers.Http.AspNetCore
      - CreativeCoders.AspNetCore
      - CreativeCoders.AspNetCore.Blazor
      - CreativeCoders.AspNetCore.TokenAuthApi
      - CreativeCoders.AspNetCore.TokenAuthApi.Jwt
      - CreativeCoders.AspNetCore.TokenAuth.Jwt
    downstream: []
```

---

### 6.3 Workflow: Manueller Major-Update-Auslöser

**`.github/workflows/trigger-major-update.yml`** im Orchestrator-Repo:

```yaml
name: Trigger Major Update

on:
  workflow_dispatch:
    inputs:
      source_repo:
        description: "Repo, das ein Major-Update erhalten hat (z.B. dotnet-libs-core)"
        required: true
        type: choice
        options:
          - dotnet-libs-core
          - dotnet-libs-io
          - dotnet-libs-config
          - dotnet-libs-dyncode
          - dotnet-libs-messaging
          - dotnet-libs-daemon
          - dotnet-libs-cli
          - dotnet-libs-data
          - dotnet-libs-net
      new_version:
        description: "Neue Major-Version (z.B. 2.0.0)"
        required: true
        type: string
      branch_name:
        description: "Branch-Name für Update-PRs (z.B. major/core-v2)"
        required: true
        default: "major/update"
        type: string

jobs:
  read-graph:
    runs-on: ubuntu-latest
    outputs:
      downstream: ${{ steps.graph.outputs.downstream }}
    steps:
      - uses: actions/checkout@v4

      - name: Read downstream repos from graph
        id: graph
        uses: mikefarah/yq@v4
        with:
          cmd: |
            DOWNSTREAM=$(yq '.repos.${{ inputs.source_repo }}.downstream | @json' config/dependency-graph.yml)
            echo "downstream=$DOWNSTREAM" >> $GITHUB_OUTPUT

  dispatch-updates:
    needs: read-graph
    runs-on: ubuntu-latest
    strategy:
      matrix:
        repo: ${{ fromJson(needs.read-graph.outputs.downstream) }}
    steps:
      - name: Dispatch update workflow to ${{ matrix.repo }}
        uses: peter-evans/repository-dispatch@v3
        with:
          token: ${{ secrets.ORCHESTRATOR_PAT }}
          repository: CreativeCoders/${{ matrix.repo }}
          event-type: major-update-available
          client-payload: |
            {
              "source_repo": "${{ inputs.source_repo }}",
              "new_version": "${{ inputs.new_version }}",
              "branch_name": "${{ inputs.branch_name }}"
            }
```

> **Voraussetzung:** Ein Personal Access Token (PAT) mit `repo`-Scope muss als Secret
> `ORCHESTRATOR_PAT` im Orchestrator-Repo hinterlegt werden. Der PAT muss Schreibzugriff
> auf alle Ziel-Repos haben. Empfehlung: GitHub App statt PAT für bessere Sicherheit.

---

### 6.4 Workflow: Major-Update empfangen (in jedem Downstream-Repo)

**`.github/workflows/receive-major-update.yml`** – wird in **jedem** der 9 Repos hinterlegt:

```yaml
name: Receive Major Update

on:
  repository_dispatch:
    types: [major-update-available]

jobs:
  create-update-branch:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Configure git
        run: |
          git config user.name "github-actions[bot]"
          git config user.email "github-actions[bot]@users.noreply.github.com"

      - name: Create update branch
        run: |
          git checkout -b ${{ github.event.client_payload.branch_name }}

      - name: Update NuGet package versions
        run: |
          SOURCE_REPO="${{ github.event.client_payload.source_repo }}"
          NEW_VERSION="${{ github.event.client_payload.new_version }}"

          # Lese alle Paketnamen des Source-Repos aus dem lokalen Kontext
          # (Liste kann auch aus dem Orchestrator-Repo per API abgerufen werden)
          # Hier: Aktualisierung per dotnet CLI
          dotnet add package "$(echo $SOURCE_REPO | sed 's/dotnet-libs-/CreativeCoders./g')" \
            --version "$NEW_VERSION" || true

          # Alternativ: Directory.Packages.props direkt aktualisieren
          # (empfohlen bei Central Package Management)

      - name: Install dependencies and build
        run: dotnet restore && dotnet build --no-restore

      - name: Run tests
        run: dotnet test --no-build

      - name: Commit changes
        run: |
          git add .
          git commit -m "chore: update ${{ github.event.client_payload.source_repo }} to v${{ github.event.client_payload.new_version }}"

      - name: Push branch
        run: git push origin ${{ github.event.client_payload.branch_name }}

      - name: Create Pull Request
        uses: peter-evans/create-pull-request@v7
        with:
          token: ${{ secrets.GITHUB_TOKEN }}
          branch: ${{ github.event.client_payload.branch_name }}
          title: "chore: Major update ${{ github.event.client_payload.source_repo }} → v${{ github.event.client_payload.new_version }}"
          body: |
            ## Major Update: ${{ github.event.client_payload.source_repo }} v${{ github.event.client_payload.new_version }}

            Dieses PR wurde automatisch durch den Update-Orchestrator erstellt.

            ### Änderungen
            - Aktualisierung der NuGet-Referenz auf `${{ github.event.client_payload.source_repo }}` v${{ github.event.client_payload.new_version }}

            ### Nächste Schritte
            - [ ] Breaking Changes im Changelog von `${{ github.event.client_payload.source_repo }}` prüfen
            - [ ] Code-Anpassungen für Breaking Changes vornehmen
            - [ ] Tests lokal ausführen und sicherstellen, dass alles grün ist
            - [ ] PR reviewen und mergen

            **Ausgelöst durch:** Update-Orchestrator
          labels: |
            major-update
            dependencies
          draft: false
```

---

### 6.5 Status-Dashboard: Überblick über alle offenen Major-Update-PRs

**`.github/workflows/status-dashboard.yml`** im Orchestrator-Repo – erstellt eine GitHub Issue
als lebendiges Dashboard für alle laufenden Major-Updates:

```yaml
name: Major Update Status Dashboard

on:
  workflow_dispatch:
    inputs:
      update_label:
        description: "Label der zu trackenden PRs (z.B. major/core-v2)"
        required: true
  schedule:
    # Täglich um 08:00 Uhr aktualisieren
    - cron: '0 8 * * *'

jobs:
  build-dashboard:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Gather PR status from all repos
        id: gather
        env:
          GH_TOKEN: ${{ secrets.ORCHESTRATOR_PAT }}
        run: |
          REPOS=(
            dotnet-libs-core
            dotnet-libs-io
            dotnet-libs-config
            dotnet-libs-messaging
            dotnet-libs-daemon
            dotnet-libs-dyncode
            dotnet-libs-cli
            dotnet-libs-data
            dotnet-libs-net
          )

          BODY="## Major Update Status Dashboard\n\n"
          BODY+="Letzte Aktualisierung: $(date -u '+%Y-%m-%d %H:%M UTC')\n\n"
          BODY+="| Repo | Offene PRs (major-update) | Status |\n"
          BODY+="|------|--------------------------|--------|\n"

          for REPO in "${REPOS[@]}"; do
            PR_COUNT=$(gh pr list \
              --repo "CreativeCoders/$REPO" \
              --label "major-update" \
              --state open \
              --json number \
              --jq 'length')

            if [ "$PR_COUNT" -eq "0" ]; then
              STATUS="✅ Aktuell"
            else
              STATUS="⏳ $PR_COUNT PR(s) offen"
            fi

            BODY+="| [$REPO](https://github.com/CreativeCoders/$REPO/pulls?q=label%3Amajor-update) | $PR_COUNT | $STATUS |\n"
          done

          echo "body<<EOF" >> $GITHUB_OUTPUT
          echo -e "$BODY" >> $GITHUB_OUTPUT
          echo "EOF" >> $GITHUB_OUTPUT

      - name: Update or create dashboard issue
        uses: peter-evans/create-or-update-comment@v4
        with:
          token: ${{ secrets.ORCHESTRATOR_PAT }}
          issue-number: 1   # Feste Issue-Nummer für das Dashboard
          body: ${{ steps.gather.outputs.body }}
```

> **Setup:** Im Orchestrator-Repo eine permanente Issue #1 mit dem Titel
> "Major Update Dashboard" anlegen. Der Workflow aktualisiert deren Kommentar täglich.

---

### 6.6 Ablauf eines Major-Updates – Schritt für Schritt

```
┌─────────────────────────────────────────────────────────────┐
│  Entwickler released dotnet-libs-core v2.0.0 auf NuGet.org  │
└──────────────────────────┬──────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────┐
│  Orchestrator-Repo: trigger-major-update.yml                │
│  (manuell via workflow_dispatch)                            │
│  Eingaben:                                                  │
│   - source_repo: dotnet-libs-core                           │
│   - new_version: 2.0.0                                      │
│   - branch_name: major/core-v2                              │
└──────────────────────────┬──────────────────────────────────┘
                           │  liest dependency-graph.yml
                           │  ermittelt downstream-Repos
                           │
           ┌───────────────┼────────────────────────┐
           │               │                        │
           ▼               ▼                        ▼
   dotnet-libs-io   dotnet-libs-config   dotnet-libs-messaging
   dotnet-libs-cli  dotnet-libs-daemon   dotnet-libs-dyncode
   dotnet-libs-data dotnet-libs-net
           │               │                        │
           └───────────────┼────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────┐
│  In jedem Downstream-Repo: receive-major-update.yml         │
│  1. Branch "major/core-v2" erstellen                        │
│  2. NuGet-Versionen in Directory.Packages.props updaten     │
│  3. dotnet restore + build + test                           │
│  4. PR erstellen mit Label "major-update"                   │
└──────────────────────────┬──────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────┐
│  Entwickler reviewt PRs in jedem Repo:                      │
│  - Breaking Changes anpassen                                │
│  - Tests reparieren                                         │
│  - PR mergen                                                │
│                                                             │
│  Orchestrator Dashboard (Issue #1) zeigt Fortschritt        │
└─────────────────────────────────────────────────────────────┘
```

---

### 6.7 Voraussetzungen im Orchestrator-Repo

| Ressource | Zweck |
|-----------|-------|
| Secret `ORCHESTRATOR_PAT` | PAT oder GitHub App Token mit `repo`-Scope auf alle Ziel-Repos (für `repository_dispatch` und PR-Erstellung) |
| Issue #1 | Permanentes Dashboard-Issue |
| `config/dependency-graph.yml` | Maschinenlesbarer Abhängigkeitsgraph (muss bei neuen Repos/Paketen gepflegt werden) |

**Empfehlung: GitHub App statt PAT**

Für Organisationen ist eine **GitHub App** mit granularen Berechtigungen besser geeignet als ein PAT:
- Berechtigungen: `contents: write`, `pull-requests: write`, `issues: write` auf alle Repos
- Kein Ablaufdatum wie bei Classic PATs
- Audit-Trail über die App statt über einen persönlichen Account

---

### 6.8 Abgrenzung: Dependabot vs. Orchestrator

| Szenario | Mechanismus | Aktion |
|----------|-------------|--------|
| `dotnet-libs-core` v1.0.3 → v1.0.4 (Patch) | Dependabot | Auto-Merge nach grünem CI |
| `dotnet-libs-core` v1.0.x → v1.1.0 (Minor) | Dependabot | Auto-Merge nach grünem CI |
| `dotnet-libs-core` v1.x → v2.0.0 (Major) | Orchestrator | PR erstellen, manueller Review |
| Externer NuGet (z.B. `Polly` v7 → v8) | Dependabot | PR erstellen, kein Auto-Merge |
| `dotnet-libs-core` auf mehrere Repos gleichzeitig | Orchestrator | Alle PRs in einem Schritt |

---

## Phase 7: Versioning & Release-Strategie

### 7.1 Versionierung

Alle Repos verwenden **Semantic Versioning** (`MAJOR.MINOR.PATCH`) via NUKE-Build.

- Packages werden bei jedem Push auf `main` mit einer Pre-Release-Version veröffentlicht
  (z. B. `1.2.0-ci.20260321.1`) – für Integration Feeds
- Offizielle Releases werden über Git-Tags (`v1.2.0`) ausgelöst

### 7.2 Abhängigkeits-Update-Kette

```
dotnet-libs-core v1.1.0 released (Minor → Dependabot)
    ↓ Dependabot erstellt PRs in:
dotnet-libs-io       → auto-merged (minor/patch)
dotnet-libs-cli      → auto-merged (minor/patch)
dotnet-libs-config   → auto-merged (minor/patch)
dotnet-libs-messaging→ auto-merged (minor/patch)
dotnet-libs-daemon   → auto-merged (minor/patch)
dotnet-libs-dyncode  → auto-merged (minor/patch)
    ↓ Neue Versionen dieser Repos werden released
    ↓ Dependabot erstellt PRs in:
dotnet-libs-data     → auto-merged (minor/patch)
dotnet-libs-net      → auto-merged (minor/patch)

dotnet-libs-core v2.0.0 released (Major → Orchestrator)
    ↓ trigger-major-update.yml (manuell ausgelöst)
    ↓ PRs in allen downstream-Repos, Label "major-update"
    ↓ Kein Auto-Merge – manuelle Review erforderlich
```

---

## Phase 8: Migrationsschritte (Reihenfolge)

Die Repos müssen **in dieser Reihenfolge** migriert werden, damit immer nur NuGet-Pakete referenziert
werden, die bereits existieren:

### Schritt 1 – `dotnet-libs-core`
- Kein NuGet-Abhängigkeiten zu anderen neuen Repos
- Kann sofort angelegt und auf NuGet veröffentlicht werden

### Schritt 2 – Parallele Migration (alle hängen nur von `dotnet-libs-core` ab)
- `dotnet-libs-io`
- `dotnet-libs-config`
- `dotnet-libs-messaging`
- `dotnet-libs-daemon`
- `dotnet-libs-dyncode`

### Schritt 3 – `dotnet-libs-cli`
- Hängt von `dotnet-libs-core` ab
- `SysConsole`-Refactoring in `Cli.ConsoleCore` durchführen
- Tests entsprechend umbenennen/anpassen

### Schritt 4 – `dotnet-libs-data`
- Hängt von `dotnet-libs-core` + `dotnet-libs-config` ab
- `CreativeCoders.Config.Base` (NuGet) für `Data.EfCore.SqlServer` referenzieren

### Schritt 5 – `dotnet-libs-net`
- Hängt von `dotnet-libs-core` + `dotnet-libs-dyncode` ab
- `CreativeCoders.DynamicCode.Proxying` (NuGet) für JsonRpc/WebApi/XmlRpc referenzieren
- `CreativeCoders.UnitTests` aufteilen: Net-Teil bleibt hier

### Schritt 6 – Altes `Core`-Repo
- Kann archiviert werden (GitHub: Settings → Archive this repository)
- README mit Hinweis auf neue Repos aktualisieren

---

## Phase 9: Checkliste je Repository

Für jedes neue Repo folgende Schritte ausführen:

- [ ] GitHub-Repository anlegen (unter `CreativeCoders`-Organisation)
- [ ] Branch Protection für `main` konfigurieren (Status Checks, Auto-Merge erlauben)
- [ ] `NUGET_ORG_TOKEN` Secret hinterlegen
- [ ] Projektdateien aus `Core` kopieren/verschieben
- [ ] `ProjectReference`-Einträge durch `PackageReference`-Einträge ersetzen
- [ ] `Directory.Packages.props` anlegen (Central Package Management)
- [ ] `Directory.Build.props` anlegen (gemeinsame Build-Properties)
- [ ] `global.json` kopieren
- [ ] `.editorconfig` kopieren
- [ ] `.gitignore` anlegen
- [ ] NUKE-Build (`build/`) konfigurieren
- [ ] GitHub Actions Workflows anlegen (`pull-request.yml`, `main.yml`, `release.yml`)
- [ ] `dependabot.yml` anlegen
- [ ] `auto-merge-dependabot.yml` anlegen
- [ ] Ersten Build und Test lokal erfolgreich durchführen
- [ ] Initialen Release (`v1.0.0`) auf NuGet.org veröffentlichen
- [ ] README mit Paket-Badges und Abhängigkeitsgraph anlegen
- [ ] `receive-major-update.yml` Workflow anlegen

**Einmalig für den Orchestrator (`dotnet-libs-update-orchestrator`):**

- [ ] Orchestrator-Repo anlegen (`CreativeCoders/dotnet-libs-update-orchestrator`)
- [ ] `config/dependency-graph.yml` anlegen und befüllen
- [ ] `trigger-major-update.yml` anlegen
- [ ] `cascade-update.yml` anlegen
- [ ] `status-dashboard.yml` anlegen
- [ ] GitHub App oder PAT mit Repo-Schreibzugriff auf alle Repos erstellen
- [ ] Secret `ORCHESTRATOR_PAT` in Orchestrator-Repo hinterlegen
- [ ] Issue #1 "Major Update Dashboard" anlegen
- [ ] Orchestrator-Repo in Branch Protection aller Repos als vertrauenswürdig konfigurieren

---

## Offene Fragen / Entscheidungsbedarf

| # | Frage | Optionen |
|---|-------|----------|
| 1 | Soll `CreativeCoders.DependencyInjection` in `core` bleiben oder eigenständig werden? | A: In `dotnet-libs-core` *(empfohlen)* / B: Eigenes Mini-Repo |
| 2 | Soll `CreativeCoders.UnitTests` gesplittet werden (Core-Teil / Net-Teil)? | A: Split in zwei Pakete / B: Komplett in `dotnet-libs-net` / C: In `dotnet-libs-core` |
| 3 | Integration Feed (GitHub Packages) oder direkt NuGet.org für CI-Builds? | A: NuGet.org mit Pre-Release / B: GitHub Packages Feed |
| 4 | Sollen die `samples/`-Projekte mit in die neuen Repos oder in ein separates `dotnet-libs-samples`-Repo? | A: Je Repo eigene Samples / B: Zentrales Samples-Repo |
| 5 | Wie wird die Versionierung initial festgelegt? Alle starten bei `1.0.0` oder vom aktuellen Core-Stand übernommen? | A: Alle bei `1.0.0` / B: Aktuelle Core-Version übernehmen |
