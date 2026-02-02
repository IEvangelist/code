using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CliContracts;

/// <summary>
/// Generates JSON schemas from CLI contract types.
/// </summary>
public sealed class CliSchema
{
    /// <summary>
    /// Gets the command name.
    /// </summary>
    [JsonPropertyName("command")]
    public required string Command { get; init; }

    /// <summary>
    /// Gets the command description.
    /// </summary>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }

    /// <summary>
    /// Gets the command options.
    /// </summary>
    [JsonPropertyName("options")]
    public required List<OptionSchema> Options { get; init; }

    /// <summary>
    /// Gets the output definitions by exit code.
    /// </summary>
    [JsonPropertyName("outputs")]
    public required Dictionary<string, OutputSchema> Outputs { get; init; }

    /// <summary>
    /// Creates a schema from a contract type.
    /// </summary>
    /// <typeparam name="T">The contract type.</typeparam>
    /// <returns>The generated schema.</returns>
    public static CliSchema From<T>() where T : class
        => From(typeof(T));

    /// <summary>
    /// Creates a schema from a contract type.
    /// </summary>
    /// <param name="contractType">The contract type.</param>
    /// <returns>The generated schema.</returns>
    public static CliSchema From(Type contractType)
    {
        var commandAttr = contractType.GetCustomAttribute<CliCommandAttribute>()
            ?? throw new InvalidOperationException(
                $"Type {contractType.Name} must have [CliCommand] attribute.");

        var properties = contractType.GetProperties();
        var options = new List<OptionSchema>();
        var outputs = new Dictionary<string, OutputSchema>();

        foreach (var property in properties)
        {
            // Check for Option attribute
            var optionAttr = property.GetCustomAttribute<OptionAttribute>();
            if (optionAttr != null)
            {
                options.Add(new OptionSchema
                {
                    Name = optionAttr.LongName,
                    ShortName = optionAttr.ShortName,
                    Type = GetTypeString(property.PropertyType),
                    Required = optionAttr.Required
                });
            }

            // Check for ExitCode attribute
            var exitCodeAttr = property.GetCustomAttribute<ExitCodeAttribute>();
            if (exitCodeAttr != null)
            {
                outputs[exitCodeAttr.Code.ToString()] = BuildOutputSchema(property.PropertyType);
            }
        }

        return new CliSchema
        {
            Command = commandAttr.Name,
            Description = commandAttr.Description,
            Options = options,
            Outputs = outputs
        };
    }

    private static OutputSchema BuildOutputSchema(Type outputType)
    {
        var schema = new OutputSchema();
        var properties = outputType.GetProperties();

        foreach (var property in properties)
        {
            var containsAttrs = property.GetCustomAttributes<ContainsAttribute>().ToList();
            if (containsAttrs.Count == 0) continue;

            var hasStdErr = property.GetCustomAttribute<StdErrAttribute>() != null;

            var texts = containsAttrs.Select(c => c.Text).ToList();

            if (hasStdErr)
            {
                schema.StderrContains ??= new List<string>();
                schema.StderrContains.AddRange(texts);
            }
            else
            {
                schema.StdoutContains ??= new List<string>();
                schema.StdoutContains.AddRange(texts);
            }
        }

        return schema;
    }

    private static string GetTypeString(Type type)
    {
        if (type == typeof(bool)) return "bool";
        if (type == typeof(int) || type == typeof(long)) return "int";
        if (type == typeof(float) || type == typeof(double)) return "number";
        if (type == typeof(string)) return "string";
        if (type.IsArray) return "array";
        return "object";
    }

    /// <summary>
    /// Saves the schema to a JSON file.
    /// </summary>
    /// <param name="path">The file path.</param>
    public void Save(string path)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var json = JsonSerializer.Serialize(this, options);
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// Converts the schema to a JSON string.
    /// </summary>
    public string ToJson()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        return JsonSerializer.Serialize(this, options);
    }
}

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
