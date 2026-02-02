namespace CliContracts.Tests;

public class ExitCodeAttributeTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(255)]
    public void Constructor_SetsCode(int code)
    {
        var attr = new ExitCodeAttribute(code);

        Assert.Equal(code, attr.Code);
    }
}
