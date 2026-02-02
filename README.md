# CliContracts

> **Define, validate, and stabilize CLI behavior using C# contracts**

`CliContracts` lets you describe a CLI's **inputs**, **outputs**, and **exit behavior** using strongly-typed C# contracts — then validate real executions against them.

Think: *OpenAPI, but for CLIs.*

---

## Installation

```bash
dotnet add package CliContracts
```

---

## Quick Start

### 1. Define a CLI Contract

```csharp
using CliContracts;

[CliCommand("build", Description = "Builds the project")]
public sealed class BuildCommand
{
    [Option("--configuration", "-c", Required = false)]
    public string Configuration { get; init; } = "Release";

    [Option("--no-restore")]
    public bool NoRestore { get; init; }

    [ExitCode(0)]
    public SuccessOutput Success { get; init; }

    [ExitCode(1)]
    public ErrorOutput Error { get; init; }
}
```

### 2. Define Output Models

```csharp
public sealed class SuccessOutput
{
    [StdOut]
    public string Output { get; init; }

    [StdOut]
    [Contains("Build succeeded")]
    public bool BuildSucceeded { get; init; }
}

public sealed class ErrorOutput
{
    [StdErr]
    public string Error { get; init; }

    [StdErr]
    [Contains("error")]
    public bool HasErrors { get; init; }
}
```

### 3. Execute & Validate

```csharp
var result = await CliContract
    .For<BuildCommand>()
    .Execute("dotnet build --configuration Release");

result.AssertValid();
```

If the CLI behavior changes unexpectedly:

- ❌ Exit code mismatch
- ❌ Missing expected output
- ❌ Broken text contract

➡️ Validation fails with detailed violations.

---

## Attributes

### `[CliCommand]`

Marks a class as a CLI command contract.

```csharp
[CliCommand("build", Description = "Builds the project")]
public sealed class BuildCommand { }
```

### `[Option]`

Defines a command-line option.

```csharp
[Option("--configuration", "-c", Required = false)]
public string Configuration { get; init; }
```

### `[ExitCode]`

Associates an output model with a specific exit code.

```csharp
[ExitCode(0)]
public SuccessOutput Success { get; init; }
```

### `[StdOut]` / `[StdErr]`

Marks a property as capturing standard output or error.

```csharp
[StdOut]
public string Output { get; init; }
```

### `[Contains]`

Specifies that output must contain specific text.

```csharp
[StdOut]
[Contains("Build succeeded")]
public bool BuildSucceeded { get; init; }
```

---

## Builder API

The contract builder supports fluent configuration:

```csharp
var result = await CliContract
    .For<BuildCommand>()
    .WithWorkingDirectory("/path/to/project")
    .WithEnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT", "1")
    .WithTimeout(TimeSpan.FromMinutes(5))
    .Execute("dotnet build");
```

---

## JSON Schema Generation

Generate a JSON schema from your contract:

```csharp
var schema = CliSchema.From<BuildCommand>();
schema.Save("build.contract.json");
```

Output:

```json
{
  "command": "build",
  "description": "Builds the project",
  "options": [
    { "name": "--configuration", "shortName": "-c", "type": "string" },
    { "name": "--no-restore", "type": "bool" }
  ],
  "outputs": {
    "0": {
      "stdoutContains": ["Build succeeded"]
    },
    "1": {
      "stderrContains": ["error"]
    }
  }
}
```

---

## Validation Results

The `CliExecutionResult<T>` provides detailed information:

```csharp
var result = await CliContract.For<BuildCommand>().Execute("dotnet build");

Console.WriteLine($"Exit Code: {result.ExitCode}");
Console.WriteLine($"Valid: {result.IsValid}");
Console.WriteLine($"StdOut: {result.StdOut}");
Console.WriteLine($"StdErr: {result.StdErr}");

foreach (var violation in result.Violations)
{
    Console.WriteLine($"Violation: {violation.Type} - {violation.Message}");
}
```

---

## Use Cases

### CI Stability Checks

Ensure CLI tools maintain consistent behavior across versions:

```csharp
[Fact]
public async Task DotnetBuild_ShouldSucceed()
{
    var result = await CliContract
        .For<BuildCommand>()
        .Execute("dotnet build");

    result.AssertValid();
}
```

### CLI Documentation

Generate machine-readable contracts for CLI tools:

```csharp
var schema = CliSchema.From<MyCliCommand>();
var json = schema.ToJson();
```

### Cross-Platform Testing

Validate CLI behavior across different environments:

```csharp
var result = await CliContract
    .For<BuildCommand>()
    .WithWorkingDirectory(testProjectPath)
    .Execute("dotnet build");

Assert.True(result.IsValid, result.GetSummary());
```

---

## Why CliContracts?

- **Strong typing over stringly CLIs** — Catch breaking changes at compile time
- **Attributes → Schema → Validation** — Single source of truth
- **CI-friendly** — Perfect for regression testing CLI behavior
- **Framework agnostic** — Works with any CLI tool
- **Extensible** — Easy to add custom validation rules

> "This doesn't replace CLI frameworks — it stabilizes them."

---

## License

MIT
