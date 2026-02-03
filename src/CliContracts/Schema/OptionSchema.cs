using System.Text.Json.Serialization;

namespace CliContracts;

/// <summary>
/// Represents an option in the schema.
/// </summary>
public sealed class OptionSchema
{
    /// <summary>
    /// Gets the option name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Gets the short option name.
    /// </summary>
    [JsonPropertyName("shortName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ShortName { get; init; }

    /// <summary>
    /// Gets the option type.
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; init; }

    /// <summary>
    /// Gets whether the option is required.
    /// </summary>
    [JsonPropertyName("required")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Required { get; init; }
}
