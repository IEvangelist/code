namespace CliContracts;

/// <summary>
/// Exception thrown when a CLI execution violates its contract.
/// </summary>
public sealed class ContractViolationException : Exception
{
    /// <summary>
    /// Gets the list of violations.
    /// </summary>
    public IReadOnlyList<ContractViolation> Violations { get; }

    /// <summary>
    /// Gets the exit code from the execution.
    /// </summary>
    public int? ExitCode { get; }

    /// <summary>
    /// Gets the standard output from the execution.
    /// </summary>
    public string? StdOut { get; }

    /// <summary>
    /// Gets the standard error from the execution.
    /// </summary>
    public string? StdErr { get; }

    /// <summary>
    /// Initializes a new instance with execution details.
    /// </summary>
    public ContractViolationException(
        IEnumerable<ContractViolation> violations,
        int exitCode,
        string stdOut,
        string stdErr)
        : base(BuildMessage(violations.ToList()))
    {
        Violations = violations.ToList().AsReadOnly();
        ExitCode = exitCode;
        StdOut = stdOut;
        StdErr = stdErr;
    }

    /// <summary>
    /// Initializes a new instance with a simple message.
    /// </summary>
    public ContractViolationException(string message) 
        : base(message)
    {
        Violations = Array.Empty<ContractViolation>();
    }

    /// <summary>
    /// Initializes a new instance with violations.
    /// </summary>
    public ContractViolationException(IEnumerable<ContractViolation> violations)
        : base(BuildMessage(violations.ToList()))
    {
        Violations = violations.ToList().AsReadOnly();
    }

    private static string BuildMessage(IList<ContractViolation> violations)
    {
        if (violations.Count == 0)
        {
            return "Contract validation failed.";
        }

        if (violations.Count == 1)
        {
            return $"Contract violation: {violations[0].Message}";
        }

        var messages = violations.Select(v => $"  - {v.Message}");
        return $"Contract violations ({violations.Count}):\n{string.Join("\n", messages)}";
    }
}
