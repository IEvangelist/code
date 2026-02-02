namespace CliContracts.Tests;

/// <summary>
/// Test command contracts used across test classes.
/// </summary>
[CliCommand("test", Description = "Test command")]
public sealed class TestCommand
{
    [Option("--name", "-n", Required = true)]
    public string Name { get; init; } = string.Empty;

    [Option("--verbose", "-v")]
    public bool Verbose { get; init; }

    [Option("--count", "-c")]
    public int Count { get; init; }

    [ExitCode(0)]
    public TestSuccessOutput Success { get; init; } = null!;

    [ExitCode(1)]
    public TestErrorOutput Error { get; init; } = null!;
}

public sealed class TestSuccessOutput
{
    [StdOut]
    public string Output { get; init; } = string.Empty;

    [StdOut]
    [Contains("Success")]
    public bool HasSuccess { get; init; }

    [StdOut]
    [Contains("completed")]
    public bool HasCompleted { get; init; }
}

public sealed class TestErrorOutput
{
    [StdErr]
    public string Error { get; init; } = string.Empty;

    [StdErr]
    [Contains("error")]
    public bool HasError { get; init; }
}

/// <summary>
/// Command without CliCommand attribute for negative tests.
/// </summary>
public sealed class InvalidCommand
{
    [Option("--test")]
    public string Test { get; init; } = string.Empty;
}

/// <summary>
/// Simple command with no options.
/// </summary>
[CliCommand("simple")]
public sealed class SimpleCommand
{
    [ExitCode(0)]
    public SimpleOutput Output { get; init; } = null!;
}

public sealed class SimpleOutput
{
    [StdOut]
    public string Text { get; init; } = string.Empty;
}

/// <summary>
/// Command with multiple exit codes.
/// </summary>
[CliCommand("multi-exit", Description = "Command with multiple exit codes")]
public sealed class MultiExitCommand
{
    [ExitCode(0)]
    public SuccessResult Success { get; init; } = null!;

    [ExitCode(1)]
    public WarningResult Warning { get; init; } = null!;

    [ExitCode(2)]
    public ErrorResult Error { get; init; } = null!;
}

public sealed class SuccessResult
{
    [StdOut]
    [Contains("OK")]
    public bool IsOk { get; init; }
}

public sealed class WarningResult
{
    [StdOut]
    [Contains("WARNING")]
    public bool HasWarning { get; init; }
}

public sealed class ErrorResult
{
    [StdErr]
    [Contains("FATAL")]
    public bool IsFatal { get; init; }
}
