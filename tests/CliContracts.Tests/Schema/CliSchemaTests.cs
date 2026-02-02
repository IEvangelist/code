namespace CliContracts.Tests;

public class CliSchemaTests
{
    [Fact]
    public void From_ExtractsCommandName()
    {
        var schema = CliSchema.From<TestCommand>();

        Assert.Equal("test", schema.Command);
    }

    [Fact]
    public void From_ExtractsDescription()
    {
        var schema = CliSchema.From<TestCommand>();

        Assert.Equal("Test command", schema.Description);
    }

    [Fact]
    public void From_ExtractsOptions()
    {
        var schema = CliSchema.From<TestCommand>();

        Assert.Equal(3, schema.Options.Count);
        
        var nameOption = schema.Options.First(o => o.Name == "--name");
        Assert.Equal("-n", nameOption.ShortName);
        Assert.Equal("string", nameOption.Type);
        Assert.True(nameOption.Required);

        var verboseOption = schema.Options.First(o => o.Name == "--verbose");
        Assert.Equal("-v", verboseOption.ShortName);
        Assert.Equal("bool", verboseOption.Type);
        Assert.False(verboseOption.Required);

        var countOption = schema.Options.First(o => o.Name == "--count");
        Assert.Equal("-c", countOption.ShortName);
        Assert.Equal("int", countOption.Type);
    }

    [Fact]
    public void From_ExtractsExitCodes()
    {
        var schema = CliSchema.From<TestCommand>();

        Assert.Equal(2, schema.Outputs.Count);
        Assert.True(schema.Outputs.ContainsKey("0"));
        Assert.True(schema.Outputs.ContainsKey("1"));
    }

    [Fact]
    public void From_ExtractsStdoutContains()
    {
        var schema = CliSchema.From<TestCommand>();

        var successOutput = schema.Outputs["0"];
        Assert.NotNull(successOutput.StdoutContains);
        Assert.Contains("Success", successOutput.StdoutContains);
        Assert.Contains("completed", successOutput.StdoutContains);
    }

    [Fact]
    public void From_ExtractsStderrContains()
    {
        var schema = CliSchema.From<TestCommand>();

        var errorOutput = schema.Outputs["1"];
        Assert.NotNull(errorOutput.StderrContains);
        Assert.Contains("error", errorOutput.StderrContains);
    }

    [Fact]
    public void From_ThrowsForInvalidCommand()
    {
        var ex = Assert.Throws<InvalidOperationException>(() => 
            CliSchema.From<InvalidCommand>());

        Assert.Contains("InvalidCommand", ex.Message);
        Assert.Contains("[CliCommand]", ex.Message);
    }

    [Fact]
    public void From_HandlesCommandWithNoDescription()
    {
        var schema = CliSchema.From<SimpleCommand>();

        Assert.Equal("simple", schema.Command);
        Assert.Null(schema.Description);
    }

    [Fact]
    public void From_HandlesMultipleExitCodes()
    {
        var schema = CliSchema.From<MultiExitCommand>();

        Assert.Equal(3, schema.Outputs.Count);
        Assert.True(schema.Outputs.ContainsKey("0"));
        Assert.True(schema.Outputs.ContainsKey("1"));
        Assert.True(schema.Outputs.ContainsKey("2"));
    }

    [Fact]
    public void ToJson_ProducesValidJson()
    {
        var schema = CliSchema.From<TestCommand>();

        var json = schema.ToJson();

        Assert.Contains("\"command\":", json);
        Assert.Contains("\"test\"", json);
        Assert.Contains("\"options\":", json);
        Assert.Contains("\"outputs\":", json);
    }

    [Fact]
    public void ToJson_IncludesDescription()
    {
        var schema = CliSchema.From<TestCommand>();

        var json = schema.ToJson();

        Assert.Contains("\"description\":", json);
        Assert.Contains("Test command", json);
    }

    [Fact]
    public void ToJson_OmitsNullDescription()
    {
        var schema = CliSchema.From<SimpleCommand>();

        var json = schema.ToJson();

        Assert.DoesNotContain("\"description\":", json);
    }

    [Fact]
    public void Save_CreatesFile()
    {
        var schema = CliSchema.From<TestCommand>();
        var tempPath = Path.Combine(Path.GetTempPath(), $"test-schema-{Guid.NewGuid()}.json");

        try
        {
            schema.Save(tempPath);

            Assert.True(File.Exists(tempPath));
            var content = File.ReadAllText(tempPath);
            Assert.Contains("\"command\":", content);
        }
        finally
        {
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }
    }
}
