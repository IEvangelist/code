namespace CliContracts;

/// <summary>
/// Marks a property as a CLI option.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class OptionAttribute : Attribute
{
    /// <summary>
    /// Gets the long option name (e.g., "--configuration").
    /// </summary>
    public string LongName { get; }

    /// <summary>
    /// Gets the short option name (e.g., "-c").
    /// </summary>
    public string? ShortName { get; }

    /// <summary>
    /// Gets or sets whether this option is required.
    /// </summary>
    public bool Required { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionAttribute"/> class.
    /// </summary>
    /// <param name="longName">The long option name.</param>
    /// <param name="shortName">The optional short option name.</param>
    public OptionAttribute(string longName, string? shortName = null)
    {
        LongName = longName;
        ShortName = shortName;
    }
}
