namespace CliContracts.Tests;

public class ContainsAttributeTests
{
    [Fact]
    public void Constructor_SetsText()
    {
        var attr = new ContainsAttribute("Build succeeded");

        Assert.Equal("Build succeeded", attr.Text);
    }

    [Fact]
    public void CaseSensitive_IsTrueByDefault()
    {
        var attr = new ContainsAttribute("test");

        Assert.True(attr.CaseSensitive);
    }

    [Fact]
    public void CaseSensitive_CanBeDisabled()
    {
        var attr = new ContainsAttribute("test") { CaseSensitive = false };

        Assert.False(attr.CaseSensitive);
    }

    [Fact]
    public void Text_CanContainSpecialCharacters()
    {
        var attr = new ContainsAttribute("error: CS1234");

        Assert.Equal("error: CS1234", attr.Text);
    }
}
