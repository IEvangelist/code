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
    /// Gets the positional arguments.
    /// </summary>
    [JsonPropertyName("arguments")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ArgumentSchema>? Arguments { get; init; }

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
        var arguments = new List<ArgumentSchema>();
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

            // Check for Argument attribute
            var argumentAttr = property.GetCustomAttribute<ArgumentAttribute>();
            if (argumentAttr != null)
            {
                arguments.Add(new ArgumentSchema
                {
                    Name = argumentAttr.Name,
                    Position = argumentAttr.Position,
                    Type = GetTypeString(property.PropertyType),
                    Required = argumentAttr.Required,
                    Description = argumentAttr.Description
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
            Arguments = arguments.Count > 0 ? arguments.OrderBy(a => a.Position).ToList() : null,
            Outputs = outputs
        };
    }

    private static OutputSchema BuildOutputSchema(Type outputType)
    {
        var schema = new OutputSchema();
        var properties = outputType.GetProperties();

        foreach (var property in properties)
        {
            var hasStdErr = property.GetCustomAttribute<StdErrAttribute>() != null;

            // Collect [Contains] attributes
            var containsAttrs = property.GetCustomAttributes<ContainsAttribute>().ToList();
            if (containsAttrs.Count > 0)
            {
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

            // Collect [MatchesPattern] attributes
            var patternAttrs = property.GetCustomAttributes<MatchesPatternAttribute>().ToList();
            if (patternAttrs.Count > 0)
            {
                var patterns = patternAttrs.Select(p => p.Pattern).ToList();

                if (hasStdErr)
                {
                    schema.StderrMatchesPatterns ??= new List<string>();
                    schema.StderrMatchesPatterns.AddRange(patterns);
                }
                else
                {
                    schema.StdoutMatchesPatterns ??= new List<string>();
                    schema.StdoutMatchesPatterns.AddRange(patterns);
                }
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
