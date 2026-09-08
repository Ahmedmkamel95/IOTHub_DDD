using TUnit.Assertions;
using TUnit.Core;

namespace CIOT.Architecture.Tests;

public sealed class BoundedContextArchitectureTests
{
    [Test]
    public void EveryBoundedContextHasAllExpectedLayers()
    {
        var missingLayers = ArchitectureTestHelpers.BoundedContexts
            .SelectMany(context =>
                new[] { "Domain", "Application", "Infrastructure", "Endpoints" }
                    .Where(layer =>
                        !ArchitectureTestHelpers.ModuleAssemblies.Any(assembly =>
                            string.Equals(
                                assembly.GetName().Name,
                                $"CIOT.Modules.{context}.{layer}",
                                StringComparison.Ordinal
                            )
                        ))
                    .Select(layer => $"CIOT.Modules.{context}.{layer}")
            )
            .ToArray();

        if (missingLayers.Length > 0)
        {
            Assert.Fail($"Missing bounded-context assemblies:{Environment.NewLine}{string.Join(Environment.NewLine, missingLayers)}");
        }
    }

    [Test]
    public void ModuleTypesRemainInsideTheirBoundedContextNamespace()
    {
        var violations = ArchitectureTestHelpers.ModuleAssemblies
            .SelectMany(assembly =>
                assembly
                    .GetTypes()
                    .Where(type => type.Namespace?.StartsWith("CIOT.Modules.", StringComparison.Ordinal) == true)
                    .Where(type =>
                        type.Namespace?.StartsWith(
                            assembly.GetName().Name![..assembly.GetName().Name!.LastIndexOf('.')].Replace(
                                ".Domain",
                                string.Empty,
                                StringComparison.Ordinal
                            ).Replace(
                                ".Application",
                                string.Empty,
                                StringComparison.Ordinal
                            ).Replace(
                                ".Infrastructure",
                                string.Empty,
                                StringComparison.Ordinal
                            ).Replace(
                                ".Endpoints",
                                string.Empty,
                                StringComparison.Ordinal
                            ),
                            StringComparison.Ordinal
                        ) != true
                    )
                    .Select(type => $"{assembly.GetName().Name}: {type.FullName}")
            )
            .ToArray();

        if (violations.Length > 0)
        {
            Assert.Fail($"Bounded-context namespace violations:{Environment.NewLine}{string.Join(Environment.NewLine, violations)}");
        }
    }
}
