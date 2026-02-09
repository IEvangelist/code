namespace CliContracts;

/// <summary>
/// Specifies that the associated output must contain the specified text.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ContainsAttribute"/> class.
/// </remarks>
/// <param name="text">The text that must be present.</param>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class ContainsAttribute(string text) : Attribute
{
    /// <summary>
    /// Gets the text that must be present in the output.
    /// </summary>
    public string Text { get; } = text;

    /// <summary>
    /// Gets or sets whether the comparison is case-sensitive.
    /// </summary>
    public bool CaseSensitive { get; init; } = true;
}
