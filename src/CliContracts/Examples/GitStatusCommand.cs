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

    [Option("--ignored")]
    public bool Ignored { get; init; }

    [ExitCode(0)]
    public GitStatusOutput Output { get; init; } = null!;

    [ExitCode(128)]
    public GitStatusError Error { get; init; } = null!;
}

/// <summary>
/// Output model for git status.
/// </summary>
public sealed class GitStatusOutput
{
    [StdOut]
    public string Output { get; init; } = string.Empty;

    [StdOut]
    [Contains("nothing to commit", CaseSensitive = false)]
    public bool IsClean { get; init; }

    [StdOut]
    [Contains("Changes not staged for commit", CaseSensitive = false)]
    public bool HasUnstagedChanges { get; init; }
}

/// <summary>
/// Output model for git status errors (e.g., not a git repository).
/// </summary>
public sealed class GitStatusError
{
    [StdErr]
    public string Error { get; init; } = string.Empty;

    [StdErr]
    [Contains("not a git repository", CaseSensitive = false)]
    public bool NotARepository { get; init; }
}
