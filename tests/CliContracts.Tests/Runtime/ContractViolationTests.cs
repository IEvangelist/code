namespace CliContracts.Tests;

public class ContractViolationTests
{
    [Fact]
    public void ToString_IncludesTypeAndMessage()
    {
        var violation = new ContractViolation
        {
            Type = ViolationType.UnexpectedExitCode,
            Message = "Expected 0, got 1"
        };

        var str = violation.ToString();

        Assert.Contains("UnexpectedExitCode", str);
        Assert.Contains("Expected 0, got 1", str);
    }

    [Fact]
    public void PropertyName_CanBeSet()
    {
        var violation = new ContractViolation
        {
            Type = ViolationType.MissingExpectedOutput,
            Message = "Missing output",
            PropertyName = "BuildSucceeded"
        };

        Assert.Equal("BuildSucceeded", violation.PropertyName);
    }

    [Fact]
    public void PropertyName_IsNullByDefault()
    {
        var violation = new ContractViolation
        {
            Type = ViolationType.MissingExpectedOutput,
            Message = "Missing output"
        };

        Assert.Null(violation.PropertyName);
    }
}

public class ViolationTypeTests
{
    [Fact]
    public void AllViolationTypesExist()
    {
        var values = Enum.GetValues<ViolationType>();

        Assert.Contains(ViolationType.UnexpectedExitCode, values);
        Assert.Contains(ViolationType.MissingExpectedOutput, values);
        Assert.Contains(ViolationType.ForbiddenOutput, values);
        Assert.Contains(ViolationType.MissingRequiredOption, values);
        Assert.Contains(ViolationType.InvalidOutputFormat, values);
    }
}
