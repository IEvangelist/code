namespace CliContracts.Examples;

/// <summary>
/// Example: Docker image list command contract.
/// Demonstrates positional arguments, regex pattern matching, and multi-exit-code handling.
/// </summary>
[CliCommand("docker image ls", Description = "List Docker images")]
public sealed class DockerImageListCommand
{
    [Argument(0, "repository", Required = false, Description = "Filter by repository name")]
    public string? Repository { get; init; }

    [Option("--format", "-f", Required = false)]
    public string? Format { get; init; }

    [Option("--filter", Required = false)]
    public string? Filter { get; init; }

    [Option("--no-trunc")]
    public bool NoTrunc { get; init; }

    [Option("--quiet", "-q")]
    public bool Quiet { get; init; }

    [Option("--digests")]
    public bool Digests { get; init; }

    [ExitCode(0)]
    public DockerImageListOutput Success { get; init; } = null!;

    [ExitCode(1)]
    public DockerImageListError Error { get; init; } = null!;
}

/// <summary>
/// Output model for a successful docker image list.
/// Uses regex patterns to validate tabular output structure.
/// </summary>
public sealed class DockerImageListOutput
{
    [StdOut]
    public string Output { get; init; } = string.Empty;

    /// <summary>
    /// Validates the header row of the default table output format.
    /// </summary>
    [StdOut]
    [MatchesPattern(@"^REPOSITORY\s+TAG\s+IMAGE ID\s+CREATED\s+SIZE$",
        Description = "Table header row",
        IgnoreCase = false,
        Multiline = true)]
    public bool HasTableHeader { get; init; }

    /// <summary>
    /// Validates that image rows conform to the expected columnar format.
    /// Each row should have: repo, tag, 12+ char hex ID, age, and size.
    /// </summary>
    [StdOut]
    [MatchesPattern(@"^\S+\s+\S+\s+[0-9a-f]{12,}\s+.+\s+[\d.]+ ?[kKMGT]?i?B$",
        Description = "Image row format: repo, tag, ID, created, size",
        Multiline = true)]
    public bool HasValidImageRows { get; init; }

    /// <summary>
    /// Validates that image IDs are valid SHA-256 prefix hex strings.
    /// </summary>
    [StdOut]
    [MatchesPattern(@"[0-9a-f]{12}",
        Description = "At least one valid image ID present")]
    public bool HasImageIds { get; init; }
}

/// <summary>
/// Output model for docker image list errors.
/// </summary>
public sealed class DockerImageListError
{
    [StdErr]
    public string Error { get; init; } = string.Empty;

    [StdErr]
    [Contains("Cannot connect to the Docker daemon", CaseSensitive = false)]
    public bool DaemonNotRunning { get; init; }

    [StdErr]
    [MatchesPattern(@"Error response from daemon:.*",
        Description = "Daemon error response",
        IgnoreCase = true)]
    public bool HasDaemonError { get; init; }
}
