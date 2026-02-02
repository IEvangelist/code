namespace CliContracts.Tests;

public class CliContractBuilderTests
{
    [Fact]
    public void WithWorkingDirectory_ReturnsBuilder()
    {
        var builder = new CliContractBuilder<TestCommand>();

        var result = builder.WithWorkingDirectory("/tmp");

        Assert.Same(builder, result);
    }

    [Fact]
    public void WithEnvironmentVariable_ReturnsBuilder()
    {
        var builder = new CliContractBuilder<TestCommand>();

        var result = builder.WithEnvironmentVariable("KEY", "VALUE");

        Assert.Same(builder, result);
    }

    [Fact]
    public void WithTimeout_ReturnsBuilder()
    {
        var builder = new CliContractBuilder<TestCommand>();

        var result = builder.WithTimeout(TimeSpan.FromMinutes(5));

        Assert.Same(builder, result);
    }

    [Fact]
    public void FluentChaining_Works()
    {
        var builder = new CliContractBuilder<TestCommand>()
            .WithWorkingDirectory("/tmp")
            .WithEnvironmentVariable("ENV1", "VAL1")
            .WithEnvironmentVariable("ENV2", "VAL2")
            .WithTimeout(TimeSpan.FromSeconds(60));

        Assert.NotNull(builder);
    }

    [Fact]
    public async Task Execute_ThrowsForInvalidCommand()
    {
        var builder = new CliContractBuilder<InvalidCommand>();

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => builder.Execute("test"));

        Assert.Contains("InvalidCommand", ex.Message);
        Assert.Contains("[CliCommand]", ex.Message);
    }

    [Fact]
    public async Task Execute_RunsSimpleCommand()
    {
        // Use a simple cross-platform command that should work
        var builder = CliContract.For<SimpleCommand>();

        // This test verifies the execution pipeline works
        // Using 'dotnet --version' as a simple command that exists
        var result = await builder.Execute("dotnet --version");

        Assert.NotNull(result);
        Assert.NotNull(result.StdOut);
        Assert.NotNull(result.StdErr);
    }

    [Fact]
    public async Task Execute_CapturesExitCode()
    {
        var builder = CliContract.For<SimpleCommand>();

        var result = await builder.Execute("dotnet --version");

        Assert.Equal(0, result.ExitCode);
    }

    [Fact]
    public async Task Execute_CapturesStdout()
    {
        var builder = CliContract.For<SimpleCommand>();

        var result = await builder.Execute("dotnet --version");

        // dotnet --version outputs the version number to stdout
        Assert.False(string.IsNullOrWhiteSpace(result.StdOut));
    }
}
