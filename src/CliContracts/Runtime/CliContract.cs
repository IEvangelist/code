namespace CliContracts;

/// <summary>
/// Entry point for creating CLI contract builders.
/// </summary>
public static class CliContract
{
    /// <summary>
    /// Creates a contract builder for the specified command type.
    /// </summary>
    /// <typeparam name="T">The command contract type.</typeparam>
    /// <returns>A new contract builder.</returns>
    public static CliContractBuilder<T> For<T>() where T : class
        => new();
}
