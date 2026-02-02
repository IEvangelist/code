namespace CliContracts;

/// <summary>
/// Marks a property as capturing standard output.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class StdOutAttribute : Attribute
{
}
