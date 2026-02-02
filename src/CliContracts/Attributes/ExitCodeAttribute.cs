namespace CliContracts;

/// <summary>
/// Marks a property as an exit code output contract.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ExitCodeAttribute : Attribute
{
    /// <summary>
    /// Gets the expected exit code.
    /// </summary>
    public int Code { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExitCodeAttribute"/> class.
    /// </summary>
    /// <param name="code">The expected exit code.</param>
    public ExitCodeAttribute(int code) => Code = code;
}
