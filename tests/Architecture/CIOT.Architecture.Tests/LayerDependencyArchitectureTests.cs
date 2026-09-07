using TUnit.Assertions;
using TUnit.Core;

namespace CIOT.Architecture.Tests;

public sealed class LayerDependencyArchitectureTests
{
    [Test]
    public void DomainAssembliesDoNotReferenceApplicationInfrastructureOrEndpoints()
    {
        var violations = ArchitectureTestHelpers.ModuleAssemblies
            .Where(assembly => assembly.GetName().Name?.EndsWith(".Domain", StringComparison.Ordinal) == true)
            .SelectMany(assembly =>
                ArchitectureTestHelpers
                    .GetModuleAssemblyReferences(assembly)
                    .Where(reference =>
                        reference.EndsWith(".Application", StringComparison.Ordinal)
                        || reference.EndsWith(".Infrastructure", StringComparison.Ordinal)
                        || reference.EndsWith(".Endpoints", StringComparison.Ordinal)
                    )
                    .Select(reference => $"{assembly.GetName().Name} -> {reference}")
            )
            .ToArray();

        if (violations.Length > 0)
        {
            Assert.Fail($"Domain dependency violations:{Environment.NewLine}{string.Join(Environment.NewLine, violations)}");
        }
    }

    [Test]
    public void ModuleAssembliesDoNotReferenceAnotherBoundedContext()
    {
        var violations = ArchitectureTestHelpers.ModuleAssemblies
            .SelectMany(assembly =>
                ArchitectureTestHelpers
                    .GetModuleAssemblyReferences(assembly)
                    .Where(reference =>
                        !reference.StartsWith(
                            assembly.GetName().Name!.Split('.')[..3].Aggregate((left, right) => $"{left}.{right}"),
                            StringComparison.Ordinal
                        )
                    )
                    .Select(reference => $"{assembly.GetName().Name} -> {reference}")
            )
            .ToArray();

        if (violations.Length > 0)
        {
            Assert.Fail($"Cross-bounded-context dependency violations:{Environment.NewLine}{string.Join(Environment.NewLine, violations)}");
        }
    }
}
