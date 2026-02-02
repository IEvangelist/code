namespace CliContracts.Tests;

public class OptionAttributeTests
{
    [Fact]
    public void Constructor_SetsLongName()
    {
        var attr = new OptionAttribute("--configuration");

        Assert.Equal("--configuration", attr.LongName);
        Assert.Null(attr.ShortName);
    }

    [Fact]
    public void Constructor_SetsBothNames()
    {
        var attr = new OptionAttribute("--configuration", "-c");

        Assert.Equal("--configuration", attr.LongName);
        Assert.Equal("-c", attr.ShortName);
    }

    [Fact]
    public void Required_IsFalseByDefault()
    {
        var attr = new OptionAttribute("--test");

        Assert.False(attr.Required);
    }

    [Fact]
    public void Required_CanBeSet()
    {
        var attr = new OptionAttribute("--test") { Required = true };

        Assert.True(attr.Required);
    }
}
