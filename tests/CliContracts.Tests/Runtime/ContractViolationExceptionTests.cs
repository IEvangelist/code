namespace CliContracts.Tests;

public class ContractViolationExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_SetsMessage()
    {
        var ex = new ContractViolationException("Test error");

        Assert.Equal("Test error", ex.Message);
        Assert.Empty(ex.Violations);
    }

    [Fact]
    public void Constructor_WithViolations_SetsViolations()
    {
        var violations = new List<ContractViolation>
        {
            new ContractViolation
            {
                Type = ViolationType.UnexpectedExitCode,
                Message = "Exit code mismatch"
            }
        };

        var ex = new ContractViolationException(violations);

        Assert.Single(ex.Violations);
        Assert.Contains("Exit code mismatch", ex.Message);
    }

    [Fact]
    public void Constructor_WithExecutionDetails_SetsAllProperties()
    {
        var violations = new List<ContractViolation>
        {
            new ContractViolation
            {
                Type = ViolationType.MissingExpectedOutput,
                Message = "Missing output"
            }
        };

        var ex = new ContractViolationException(violations, 1, "stdout", "stderr");

        Assert.Single(ex.Violations);
        Assert.Equal(1, ex.ExitCode);
        Assert.Equal("stdout", ex.StdOut);
        Assert.Equal("stderr", ex.StdErr);
    }

    [Fact]
    public void Message_FormatsMultipleViolations()
    {
        var violations = new List<ContractViolation>
        {
            new ContractViolation { Type = ViolationType.UnexpectedExitCode, Message = "Error 1" },
            new ContractViolation { Type = ViolationType.MissingExpectedOutput, Message = "Error 2" }
        };

        var ex = new ContractViolationException(violations);

        Assert.Contains("2", ex.Message);
        Assert.Contains("Error 1", ex.Message);
        Assert.Contains("Error 2", ex.Message);
    }

    [Fact]
    public void Message_HandlesSingleViolation()
    {
        var violations = new List<ContractViolation>
        {
            new ContractViolation { Type = ViolationType.UnexpectedExitCode, Message = "Single error" }
        };

        var ex = new ContractViolationException(violations);

        Assert.Contains("Single error", ex.Message);
        Assert.Contains("violation", ex.Message.ToLowerInvariant());
    }

    [Fact]
    public void Violations_IsReadOnly()
    {
        var violations = new List<ContractViolation>
        {
            new ContractViolation { Type = ViolationType.UnexpectedExitCode, Message = "Error" }
        };

        var ex = new ContractViolationException(violations);

        Assert.IsAssignableFrom<IReadOnlyList<ContractViolation>>(ex.Violations);
    }
}
