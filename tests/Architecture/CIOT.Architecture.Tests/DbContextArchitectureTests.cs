using Microsoft.EntityFrameworkCore;
using TUnit.Assertions;
using TUnit.Core;

namespace CIOT.Architecture.Tests;

public sealed class DbContextArchitectureTests
{
    [Test]
    public void EveryBoundedContextHasExactlyOneDbContextInInfrastructure()
    {
        var violations = ArchitectureTestHelpers.BoundedContexts
            .Select(context =>
            {
                var dbContexts = ArchitectureTestHelpers
                    .GetModuleTypes("Infrastructure")
                    .Where(type =>
                        type.IsClass
                        && !type.IsAbstract
                        && typeof(DbContext).IsAssignableFrom(type)
                        && type.Namespace?.StartsWith(
                            $"CIOT.Modules.{context}.Infrastructure",
                            StringComparison.Ordinal
                        ) == true
                    )
                    .ToArray();

                return dbContexts.Length == 1
                    ? null
                    : $"CIOT.Modules.{context}.Infrastructure contains {dbContexts.Length} DbContexts";
            })
            .Where(violation => violation is not null)
            .Select(violation => violation!)
            .ToArray();

        if (violations.Length > 0)
        {
            Assert.Fail($"DbContext boundary violations:{Environment.NewLine}{string.Join(Environment.NewLine, violations)}");
        }
    }
}
