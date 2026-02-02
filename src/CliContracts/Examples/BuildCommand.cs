namespace CliContracts.Examples;

/// <summary>
/// Example: Build command contract definition.
/// </summary>
[CliCommand("build", Description = "Builds the project")]
public sealed class BuildCommand
{
    [Option("--configuration", "-c", Required = false)]
    public string Configuration { get; init; } = "Release";

    [Option("--no-restore")]
    public bool NoRestore { get; init; }

    [ExitCode(0)]
    public SuccessOutput Success { get; init; } = null!;

    [ExitCode(1)]
    public ErrorOutput Error { get; init; } = null!;
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
}

/// <summary>
/// Output model for failed builds.
/// </summary>
public sealed class ErrorOutput
{
    [StdErr]
    public string Error { get; init; } = string.Empty;

    [StdErr]
    [Contains("error")]
    public bool HasErrors { get; init; }
}
