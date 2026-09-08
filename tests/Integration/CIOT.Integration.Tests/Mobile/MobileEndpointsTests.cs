using System.Net;
using System.Net.Http.Json;
using CIOT.Integration.Tests.Infrastructure;
using TUnit.Core;

namespace CIOT.Integration.Tests.Mobile;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class MobileEndpointsTests(ApiAppFixture fixture)
{
    [Test]
    public async Task SyncOfflineBatch_PersistsAndReportsProcessedActions(
        CancellationToken cancellationToken
    )
    {
        var response = await fixture.CreateApiClient().PostAsJsonAsync(
            "/api/mobile/sync-batch",
            new
            {
                technicianUserId = Guid.NewGuid(),
                deviceClientSessionId = $"mobile-session-{Guid.NewGuid():N}",
                actions = new[]
                {
                    new { clientActionId = "action-1", actionType = "ReplaceDevice", payloadJson = "{}" },
                    new { clientActionId = "action-2", actionType = "InstallFilter", payloadJson = """{"filter":"F1"}""" }
                }
            },
            cancellationToken
        );

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var document = await ApiIntegrationAssertions.ReadJsonAsync(response, cancellationToken);
        await Assert.That(document.GetProperty("batchId").GetGuid()).IsNotEqualTo(Guid.Empty);
        await Assert.That(document.GetProperty("processedCount").GetInt32()).IsEqualTo(2);
        await Assert.That(document.GetProperty("success").GetBoolean()).IsTrue();
    }
}
