namespace CliContracts.Tests;

public class CliContractTests
{
    [Fact]
    public void For_ReturnsBuilder()
    {
        var builder = CliContract.For<TestCommand>();

        Assert.NotNull(builder);
        Assert.IsType<CliContractBuilder<TestCommand>>(builder);
    }

    [Fact]
    public void For_CanBeChained()
    {
        var builder = CliContract.For<TestCommand>()
            .WithWorkingDirectory("/tmp")
            .WithTimeout(TimeSpan.FromMinutes(1))
            .WithEnvironmentVariable("TEST", "value");

        Assert.NotNull(builder);
    }
}
