namespace CliContracts;

/// <summary>
/// Marks a property as an exit code output contract.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ExitCodeAttribute"/> class.
/// </remarks>
/// <param name="code">The expected exit code.</param>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ExitCodeAttribute(int code) : Attribute
{
    /// <summary>
    /// Gets the expected exit code.
    /// </summary>
    public int Code { get; } = code;
}
