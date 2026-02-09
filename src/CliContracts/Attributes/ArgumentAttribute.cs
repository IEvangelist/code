namespace CliContracts;

/// <summary>
/// Marks a property as a positional CLI argument (as opposed to a named option).
/// Positional arguments are passed without a flag prefix and are identified by their position.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ArgumentAttribute"/> class.
/// </remarks>
/// <param name="position">The zero-based position of the argument.</param>
/// <param name="name">The display name of the argument.</param>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ArgumentAttribute(int position, string name) : Attribute
{
    /// <summary>
    /// Gets the zero-based position of this argument in the command.
    /// </summary>
    public int Position { get; } = position;

    /// <summary>
    /// Gets the display name for this argument (used in help text and schema).
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets or sets whether this argument is required. Defaults to true.
    /// </summary>
    public bool Required { get; init; } = true;

    /// <summary>
    /// Gets or sets the description of this argument.
    /// </summary>
    public string? Description { get; init; }
}
