namespace CliContracts;

/// <summary>
/// Specifies that the associated output must contain the specified text.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class ContainsAttribute : Attribute
{
    /// <summary>
    /// Gets the text that must be present in the output.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Gets or sets whether the comparison is case-sensitive.
    /// </summary>
    public bool CaseSensitive { get; init; } = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContainsAttribute"/> class.
    /// </summary>
    /// <param name="text">The text that must be present.</param>
    public ContainsAttribute(string text) => Text = text;
}
