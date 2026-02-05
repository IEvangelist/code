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
}
