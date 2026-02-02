namespace CliContracts;

/// <summary>
/// Represents a contract violation.
/// </summary>
public sealed class ContractViolation
{
    /// <summary>
    /// Gets the type of violation.
    /// </summary>
    public required ViolationType Type { get; init; }

    /// <summary>
    /// Gets the violation message.
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// Gets the property name associated with the violation, if any.
    /// </summary>
    public string? PropertyName { get; init; }

    public override string ToString() => $"[{Type}] {Message}";
}

/// <summary>
/// The types of contract violations.
/// </summary>
public enum ViolationType
{
    /// <summary>
    /// The exit code was not defined in the contract.
    /// </summary>
    UnexpectedExitCode,

    /// <summary>
    /// Expected output text was not found.
    /// </summary>
    MissingExpectedOutput,

    /// <summary>
    /// Forbidden output text was found.
    /// </summary>
    ForbiddenOutput,

    /// <summary>
    /// Required option was not provided.
    /// </summary>
    MissingRequiredOption,

    /// <summary>
    /// Output format did not match expected pattern.
    /// </summary>
    InvalidOutputFormat
}
