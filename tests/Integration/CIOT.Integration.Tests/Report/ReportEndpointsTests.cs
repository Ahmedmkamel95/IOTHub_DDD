using System.Net;
using System.Net.Http.Json;
using CIOT.Integration.Tests.Infrastructure;
using TUnit.Core;

namespace CIOT.Integration.Tests.Report;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class ReportEndpointsTests(ApiAppFixture fixture)
{
    [Test]
    public async Task CreateReportDefinition_ThenListDefinitions_ReturnsPersistedDefinition(
        CancellationToken cancellationToken
    )
    {
        var client = fixture.CreateApiClient();
        var reportCode = $"RPT-{Guid.NewGuid():N}";
        var createResponse = await client.PostAsJsonAsync(
            "/api/reports/definitions",
            new
            {
                reportCode = reportCode.ToLowerInvariant(),
                displayName = "Integration Report",
                reportType = "Operational"
            },
            cancellationToken
        );

        await Assert.That(createResponse.StatusCode).IsEqualTo(HttpStatusCode.Created);
        var created = await ApiIntegrationAssertions.ReadJsonAsync(createResponse, cancellationToken);
        await Assert.That(created.GetProperty("reportCode").GetString())
            .IsEqualTo(reportCode.ToUpperInvariant());
        await Assert.That(created.GetProperty("isActive").GetBoolean()).IsTrue();

        var listResponse = await client.GetAsync("/api/reports/definitions", cancellationToken);
        await Assert.That(listResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var definitions = await ApiIntegrationAssertions.ReadJsonAsync(listResponse, cancellationToken);
        await Assert.That(
            definitions.EnumerateArray().Any(item =>
                item.GetProperty("id").GetGuid() == created.GetProperty("id").GetGuid())
        ).IsTrue();
    }
}
