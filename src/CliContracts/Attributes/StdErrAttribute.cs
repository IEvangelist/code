namespace CliContracts;

/// <summary>
/// Marks a property as capturing standard error.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class StdErrAttribute : Attribute
{
}
