namespace CliContracts.Tests;

public class CliCommandAttributeTests
{
    [Fact]
    public void Constructor_SetsName()
    {
        var attr = new CliCommandAttribute("build");

        Assert.Equal("build", attr.Name);
    }

    [Fact]
    public void Description_CanBeSet()
    {
        var attr = new CliCommandAttribute("build")
        {
            Description = "Builds the project"
        };

        Assert.Equal("build", attr.Name);
        Assert.Equal("Builds the project", attr.Description);
    }

    [Fact]
    public void Description_IsNullByDefault()
    {
        var attr = new CliCommandAttribute("test");

        Assert.Null(attr.Description);
    }
}
