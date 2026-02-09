using System.Text.Json.Serialization;

namespace CliContracts;

/// <summary>
/// Represents an output definition in the schema.
/// </summary>
public sealed class OutputSchema
{
    /// <summary>
    /// Gets the expected stdout contents.
    /// </summary>
    [JsonPropertyName("stdoutContains")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? StdoutContains { get; set; }

    /// <summary>
    /// Gets the expected stderr contents.
    /// </summary>
    [JsonPropertyName("stderrContains")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? StderrContains { get; set; }

    /// <summary>
    /// Gets the regex patterns that stdout must match.
    /// </summary>
    [JsonPropertyName("stdoutMatchesPatterns")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? StdoutMatchesPatterns { get; set; }

    /// <summary>
    /// Gets the regex patterns that stderr must match.
    /// </summary>
    [JsonPropertyName("stderrMatchesPatterns")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? StderrMatchesPatterns { get; set; }
}
