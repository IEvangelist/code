namespace CliContracts;

/// <summary>
/// Specifies that the associated output must match the given regular expression pattern.
/// Unlike <see cref="ContainsAttribute"/> which checks for literal text,
/// this attribute validates output structure using regex patterns.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MatchesPatternAttribute"/> class.
/// </remarks>
/// <param name="pattern">The regex pattern the output must match.</param>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class MatchesPatternAttribute(string pattern) : Attribute
{
    /// <summary>
    /// Gets the regex pattern that the output must match.
    /// </summary>
    public string Pattern { get; } = pattern;

    /// <summary>
    /// Gets or sets the human-readable description of what this pattern validates.
    /// Used in violation messages to provide context.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets or sets whether the pattern matching is case-insensitive. Defaults to false.
    /// </summary>
    public bool IgnoreCase { get; init; }

    /// <summary>
    /// Gets or sets whether the pattern should use multiline mode, where ^ and $
    /// match the beginning and end of each line. Defaults to true.
    /// </summary>
    public bool Multiline { get; init; } = true;
}
