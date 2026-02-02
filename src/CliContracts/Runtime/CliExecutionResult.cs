namespace CliContracts;

/// <summary>
/// Represents the result of a CLI execution validated against a contract.
/// </summary>
/// <typeparam name="T">The contract type.</typeparam>
public sealed class CliExecutionResult<T> where T : class
{
    /// <summary>
    /// Gets the process exit code.
    /// </summary>
    public required int ExitCode { get; init; }

    /// <summary>
    /// Gets the standard output content.
    /// </summary>
    public required string StdOut { get; init; }

    /// <summary>
    /// Gets the standard error content.
    /// </summary>
    public required string StdErr { get; init; }

    /// <summary>
    /// Gets the list of contract violations.
    /// </summary>
    public required List<ContractViolation> Violations { get; init; }

    /// <summary>
    /// Gets the command that was executed.
    /// </summary>
    public required string Command { get; init; }

    /// <summary>
    /// Gets the expected command from the contract.
    /// </summary>
    public required string ExpectedCommand { get; init; }

    /// <summary>
    /// Gets whether the execution was valid against the contract.
    /// </summary>
    public bool IsValid => Violations.Count == 0;

    /// <summary>
    /// Asserts that the execution is valid. Throws if there are violations.
    /// </summary>
    /// <exception cref="ContractViolationException">
    /// Thrown when there are contract violations.
    /// </exception>
    public void AssertValid()
    {
        if (!IsValid)
        {
            throw new ContractViolationException(Violations, ExitCode, StdOut, StdErr);
        }
    }

    /// <summary>
    /// Gets a summary of the execution result.
    /// </summary>
    public string GetSummary()
    {
        var builder = new System.Text.StringBuilder();
        builder.AppendLine($"Command: {Command}");
        builder.AppendLine($"Exit Code: {ExitCode}");
        builder.AppendLine($"Valid: {IsValid}");
        
        if (Violations.Count > 0)
        {
            builder.AppendLine($"Violations ({Violations.Count}):");
            foreach (var violation in Violations)
            {
                builder.AppendLine($"  - [{violation.Type}] {violation.Message}");
            }
        }

        return builder.ToString();
    }
}

/// <summary>
/// Represents a non-generic execution result for simpler scenarios.
/// </summary>
public sealed class CliExecutionResult
{
    /// <summary>
    /// Gets the process exit code.
    /// </summary>
    public required int ExitCode { get; init; }

    /// <summary>
    /// Gets the standard output content.
    /// </summary>
    public required string StdOut { get; init; }

    /// <summary>
    /// Gets the standard error content.
    /// </summary>
    public required string StdErr { get; init; }

    /// <summary>
    /// Asserts that the exit code matches the expected value.
    /// </summary>
    public void AssertExitCode(int expected)
    {
        if (ExitCode != expected)
        {
            throw new ContractViolationException(
                $"Expected exit code {expected} but got {ExitCode}");
        }
    }
}
