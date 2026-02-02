namespace CliContracts.Tests;

public class CliExecutionResultTests
{
    [Fact]
    public void IsValid_TrueWhenNoViolations()
    {
        var result = new CliExecutionResult<TestCommand>
        {
            ExitCode = 0,
            StdOut = "Success completed",
            StdErr = "",
            Violations = new List<ContractViolation>(),
            Command = "test",
            ExpectedCommand = "test"
        };

        Assert.True(result.IsValid);
    }

    [Fact]
    public void IsValid_FalseWhenHasViolations()
    {
        var result = new CliExecutionResult<TestCommand>
        {
            ExitCode = 0,
            StdOut = "",
            StdErr = "",
            Violations = new List<ContractViolation>
            {
                new ContractViolation
                {
                    Type = ViolationType.MissingExpectedOutput,
                    Message = "Missing expected output"
                }
            },
            Command = "test",
            ExpectedCommand = "test"
        };

        Assert.False(result.IsValid);
    }

    [Fact]
    public void AssertValid_DoesNotThrowWhenValid()
    {
        var result = new CliExecutionResult<TestCommand>
        {
            ExitCode = 0,
            StdOut = "Success completed",
            StdErr = "",
            Violations = new List<ContractViolation>(),
            Command = "test",
            ExpectedCommand = "test"
        };

        var exception = Record.Exception(() => result.AssertValid());

        Assert.Null(exception);
    }

    [Fact]
    public void AssertValid_ThrowsWhenInvalid()
    {
        var result = new CliExecutionResult<TestCommand>
        {
            ExitCode = 0,
            StdOut = "",
            StdErr = "",
            Violations = new List<ContractViolation>
            {
                new ContractViolation
                {
                    Type = ViolationType.MissingExpectedOutput,
                    Message = "Expected stdout to contain \"Success\""
                }
            },
            Command = "test",
            ExpectedCommand = "test"
        };

        Assert.Throws<ContractViolationException>(() => result.AssertValid());
    }

    [Fact]
    public void GetSummary_IncludesCommand()
    {
        var result = new CliExecutionResult<TestCommand>
        {
            ExitCode = 0,
            StdOut = "output",
            StdErr = "",
            Violations = new List<ContractViolation>(),
            Command = "test --name foo",
            ExpectedCommand = "test"
        };

        var summary = result.GetSummary();

        Assert.Contains("test --name foo", summary);
    }

    [Fact]
    public void GetSummary_IncludesExitCode()
    {
        var result = new CliExecutionResult<TestCommand>
        {
            ExitCode = 42,
            StdOut = "",
            StdErr = "",
            Violations = new List<ContractViolation>(),
            Command = "test",
            ExpectedCommand = "test"
        };

        var summary = result.GetSummary();

        Assert.Contains("42", summary);
    }

    [Fact]
    public void GetSummary_IncludesViolations()
    {
        var result = new CliExecutionResult<TestCommand>
        {
            ExitCode = 0,
            StdOut = "",
            StdErr = "",
            Violations = new List<ContractViolation>
            {
                new ContractViolation
                {
                    Type = ViolationType.MissingExpectedOutput,
                    Message = "Missing output text"
                }
            },
            Command = "test",
            ExpectedCommand = "test"
        };

        var summary = result.GetSummary();

        Assert.Contains("Violations", summary);
        Assert.Contains("Missing output text", summary);
    }
}
