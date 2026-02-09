namespace CliContracts;

/// <summary>
/// Marks a class as a CLI command contract.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CliCommandAttribute"/> class.
/// </remarks>
/// <param name="name">The command name.</param>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class CliCommandAttribute(string name) : Attribute
{
    /// <summary>
    /// Gets the command name.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets or sets the command description.
    /// </summary>
    public string? Description { get; init; }
}
