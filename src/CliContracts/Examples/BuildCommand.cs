namespace CliContracts.Examples;

/// <summary>
/// Example: Build command contract definition.
/// </summary>
[CliCommand("build", Description = "Builds the project")]
public sealed class BuildCommand
{
    [Option("--configuration", "-c", Required = false)]
    public string Configuration { get; init; } = "Release";

    [Option("--verbosity", "-v", Required = false)]
    public string Verbosity { get; init; } = "minimal";

    [Option("--no-restore")]
    public bool NoRestore { get; init; }

    [Option("--no-incremental")]
    public bool NoIncremental { get; init; }

    [ExitCode(0)]
    public SuccessOutput Success { get; init; } = null!;

    [ExitCode(1)]
    public ErrorOutput Error { get; init; } = null!;

    [ExitCode(2)]
    public WarningOutput Warning { get; init; } = null!;
}

/// <summary>
/// Output model for successful builds.
/// </summary>
public sealed class SuccessOutput
{
    [StdOut]
    public string Output { get; init; } = string.Empty;

    [StdOut]
    [Contains("Build succeeded")]
    public bool BuildSucceeded { get; init; }

    [StdOut]
    [Contains("0 Warning(s)", CaseSensitive = false)]
    public bool NoWarnings { get; init; }
}

/// <summary>
/// Output model for builds that succeeded with warnings.
/// </summary>
public sealed class WarningOutput
{
    [StdOut]
    public string Output { get; init; } = string.Empty;

    [StdOut]
    [Contains("Build succeeded")]
    public bool BuildSucceeded { get; init; }

    [StdErr]
    [Contains("warning", CaseSensitive = false)]
    public bool HasWarnings { get; init; }
}

/// <summary>
/// Output model for failed builds.
/// </summary>
public sealed class ErrorOutput
{
    [StdErr]
    public string Error { get; init; } = string.Empty;

    [StdErr]
    [Contains("error", CaseSensitive = false)]
    public bool HasErrors { get; init; }
}
