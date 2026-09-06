using ReflectionAssembly = System.Reflection.Assembly;

namespace CIOT.Architecture.Tests;

internal static class ArchitectureTestHelpers
{
    internal static readonly string[] BoundedContexts =
    [
        "Admin",
        "Asset",
        "Audit",
        "Catalog",
        "CustomerOutlet",
        "Devices",
        "Identity",
        "Integration",
        "LocalAdapter",
        "Mobile",
        "Org",
        "Provisioning",
        "Report",
        "Telemetry",
    ];

    internal static IReadOnlyList<ReflectionAssembly> ModuleAssemblies { get; } =
        BoundedContexts
            .SelectMany(context =>
                new[]
                {
                    ReflectionAssembly.Load($"CIOT.Modules.{context}.Domain"),
                    ReflectionAssembly.Load($"CIOT.Modules.{context}.Application"),
                    ReflectionAssembly.Load($"CIOT.Modules.{context}.Infrastructure"),
                    ReflectionAssembly.Load($"CIOT.Modules.{context}.Endpoints"),
                }
            )
            .ToArray();

    internal static IEnumerable<Type> GetModuleTypes(string layer)
    {
        return ModuleAssemblies
            .Where(assembly => assembly.GetName().Name?.EndsWith($".{layer}", StringComparison.Ordinal) == true)
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.Namespace?.StartsWith("CIOT.Modules.", StringComparison.Ordinal) == true);
    }

    internal static IEnumerable<string> GetModuleAssemblyReferences(ReflectionAssembly assembly)
    {
        return assembly
            .GetReferencedAssemblies()
            .Select(reference => reference.Name)
            .Where(name => name?.StartsWith("CIOT.Modules.", StringComparison.Ordinal) == true)
            .Where(name => name is not null)
            .Select(name => name!)
            .Order(StringComparer.Ordinal);
    }
}
