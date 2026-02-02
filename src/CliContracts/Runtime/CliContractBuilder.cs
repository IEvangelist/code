using System.Diagnostics;
using System.Reflection;

namespace CliContracts;

/// <summary>
/// Builds and executes CLI contracts.
/// </summary>
/// <typeparam name="T">The command contract type.</typeparam>
public sealed class CliContractBuilder<T> where T : class
{
    private string? _workingDirectory;
    private readonly Dictionary<string, string> _environmentVariables = new();
    private TimeSpan _timeout = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Sets the working directory for command execution.
    /// </summary>
    /// <param name="path">The working directory path.</param>
    /// <returns>This builder for chaining.</returns>
    public CliContractBuilder<T> WithWorkingDirectory(string path)
    {
        _workingDirectory = path;
        return this;
    }

    /// <summary>
    /// Adds an environment variable for command execution.
    /// </summary>
    /// <param name="name">The variable name.</param>
    /// <param name="value">The variable value.</param>
    /// <returns>This builder for chaining.</returns>
    public CliContractBuilder<T> WithEnvironmentVariable(string name, string value)
    {
        _environmentVariables[name] = value;
        return this;
    }

    /// <summary>
    /// Sets the execution timeout.
    /// </summary>
    /// <param name="timeout">The timeout duration.</param>
    /// <returns>This builder for chaining.</returns>
    public CliContractBuilder<T> WithTimeout(TimeSpan timeout)
    {
        _timeout = timeout;
        return this;
    }

    /// <summary>
    /// Executes the command and validates against the contract.
    /// </summary>
    /// <param name="command">The full command to execute.</param>
    /// <returns>The execution result.</returns>
    public async Task<CliExecutionResult<T>> Execute(string command)
    {
        var contractType = typeof(T);
        var commandAttr = contractType.GetCustomAttribute<CliCommandAttribute>();
        
        if (commandAttr == null)
        {
            throw new InvalidOperationException(
                $"Type {contractType.Name} must be decorated with [CliCommand] attribute.");
        }

        // Parse command into executable and arguments
        var parts = ParseCommand(command);
        var executable = parts.Executable;
        var arguments = parts.Arguments;

        // Execute the process
        var (exitCode, stdOut, stdErr) = await RunProcessAsync(executable, arguments);

        // Validate against contract
        var violations = ValidateContract(contractType, exitCode, stdOut, stdErr);

        return new CliExecutionResult<T>
        {
            ExitCode = exitCode,
            StdOut = stdOut,
            StdErr = stdErr,
            Violations = violations,
            Command = command,
            ExpectedCommand = commandAttr.Name
        };
    }

    private (string Executable, string Arguments) ParseCommand(string command)
    {
        var trimmed = command.Trim();
        var firstSpace = trimmed.IndexOf(' ');
        
        if (firstSpace == -1)
        {
            return (trimmed, string.Empty);
        }

        return (trimmed[..firstSpace], trimmed[(firstSpace + 1)..]);
    }

    private async Task<(int ExitCode, string StdOut, string StdErr)> RunProcessAsync(
        string executable, 
        string arguments)
    {
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = executable,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = _workingDirectory ?? Environment.CurrentDirectory
        };

        foreach (var (name, value) in _environmentVariables)
        {
            process.StartInfo.EnvironmentVariables[name] = value;
        }

        var stdOutBuilder = new System.Text.StringBuilder();
        var stdErrBuilder = new System.Text.StringBuilder();

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                stdOutBuilder.AppendLine(e.Data);
            }
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                stdErrBuilder.AppendLine(e.Data);
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        using var cts = new CancellationTokenSource(_timeout);
        
        try
        {
            await process.WaitForExitAsync(cts.Token);
        }
        catch (OperationCanceledException)
        {
            process.Kill(entireProcessTree: true);
            throw new TimeoutException(
                $"Command execution exceeded timeout of {_timeout.TotalSeconds} seconds.");
        }

        return (process.ExitCode, stdOutBuilder.ToString(), stdErrBuilder.ToString());
    }

    private List<ContractViolation> ValidateContract(
        Type contractType, 
        int exitCode, 
        string stdOut, 
        string stdErr)
    {
        var violations = new List<ContractViolation>();
        var properties = contractType.GetProperties();

        // Find the output type for this exit code
        var exitCodeProperty = properties
            .FirstOrDefault(p => 
            {
                var attr = p.GetCustomAttribute<ExitCodeAttribute>();
                return attr != null && attr.Code == exitCode;
            });

        if (exitCodeProperty == null)
        {
            // Check if there are any exit code definitions
            var definedExitCodes = properties
                .Select(p => p.GetCustomAttribute<ExitCodeAttribute>())
                .Where(a => a != null)
                .Select(a => a!.Code)
                .ToList();

            if (definedExitCodes.Count > 0)
            {
                violations.Add(new ContractViolation
                {
                    Type = ViolationType.UnexpectedExitCode,
                    Message = $"Exit code {exitCode} is not defined in the contract. " +
                              $"Expected one of: {string.Join(", ", definedExitCodes)}"
                });
            }

            return violations;
        }

        // Validate the output type properties
        var outputType = exitCodeProperty.PropertyType;
        ValidateOutputType(outputType, stdOut, stdErr, violations);

        return violations;
    }

    private void ValidateOutputType(
        Type outputType, 
        string stdOut, 
        string stdErr, 
        List<ContractViolation> violations)
    {
        var properties = outputType.GetProperties();

        foreach (var property in properties)
        {
            // Check [Contains] attributes
            var containsAttrs = property.GetCustomAttributes<ContainsAttribute>();
            
            foreach (var contains in containsAttrs)
            {
                var hasStdOut = property.GetCustomAttribute<StdOutAttribute>() != null;
                var hasStdErr = property.GetCustomAttribute<StdErrAttribute>() != null;

                // If property has StdOut attribute, check stdout
                // If property has StdErr attribute, check stderr
                // If property has Contains but no stream attribute, check stdout by default
                string textToCheck = hasStdErr ? stdErr : stdOut;

                var comparison = contains.CaseSensitive 
                    ? StringComparison.Ordinal 
                    : StringComparison.OrdinalIgnoreCase;

                if (!textToCheck.Contains(contains.Text, comparison))
                {
                    var stream = hasStdErr ? "stderr" : "stdout";
                    violations.Add(new ContractViolation
                    {
                        Type = ViolationType.MissingExpectedOutput,
                        Message = $"Expected {stream} to contain \"{contains.Text}\" " +
                                  $"(property: {property.Name})"
                    });
                }
            }
        }
    }
}
