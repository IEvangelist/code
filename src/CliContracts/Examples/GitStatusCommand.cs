namespace CliContracts.Examples;

/// <summary>
/// Example: Git status command contract.
/// </summary>
[CliCommand("status", Description = "Shows the working tree status")]
public sealed class GitStatusCommand
{
    [Option("--short", "-s")]
    public bool Short { get; init; }

    [Option("--branch", "-b")]
    public bool Branch { get; init; }

    [Option("--porcelain")]
    public bool Porcelain { get; init; }

    [ExitCode(0)]
    public GitStatusOutput Output { get; init; } = null!;
}

/// <summary>
/// Output model for git status.
/// </summary>
public sealed class GitStatusOutput
{
    [StdOut]
    public string Output { get; init; } = string.Empty;
}
