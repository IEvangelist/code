namespace CliContracts;

/// <summary>
/// Marks a property as a CLI option.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="OptionAttribute"/> class.
/// </remarks>
/// <param name="longName">The long option name.</param>
/// <param name="shortName">The optional short option name.</param>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class OptionAttribute(string longName, string? shortName = null) : Attribute
{
    /// <summary>
    /// Gets the long option name (e.g., "--configuration").
    /// </summary>
    public string LongName { get; } = longName;

    /// <summary>
    /// Gets the short option name (e.g., "-c").
    /// </summary>
    public string? ShortName { get; } = shortName;

    /// <summary>
    /// Gets or sets whether this option is required.
    /// </summary>
    public bool Required { get; init; }
}
