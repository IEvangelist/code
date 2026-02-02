namespace CliContracts;

/// <summary>
/// Marks a class as a CLI command contract.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class CliCommandAttribute : Attribute
{
    /// <summary>
    /// Gets the command name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets the command description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CliCommandAttribute"/> class.
    /// </summary>
    /// <param name="name">The command name.</param>
    public CliCommandAttribute(string name) => Name = name;
}
