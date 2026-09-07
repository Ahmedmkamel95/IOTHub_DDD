using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CIOT.Integration.Tests.Infrastructure;
using TUnit.Core;

namespace CIOT.Integration.Tests.Org;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class OrgEndpointsTests(ApiAppFixture fixture)
{
    [Test]
    public async Task CreateCountry_ThenGetByCode_PersistsThroughApi(
        CancellationToken cancellationToken
    )
    {
        var code = $"T{Guid.NewGuid():N}"[..10].ToUpperInvariant();
        var client = fixture.CreateApiClient();

        var createResponse = await client.PostAsJsonAsync(
            "/api/org/countries",
            new
            {
                countryCode = code,
                countryName = "Integration Test Country",
                defaultTimezone = "UTC"
            },
            cancellationToken
        );

        await Assert.That(createResponse.StatusCode).IsEqualTo(HttpStatusCode.Created);

        var getResponse = await client.GetAsync($"/api/org/countries/{code}", cancellationToken);

        await Assert.That(getResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var document = await getResponse.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        await Assert.That(document.GetProperty("countryCode").GetString()).IsEqualTo(code);
        await Assert.That(document.GetProperty("countryName").GetString())
            .IsEqualTo("Integration Test Country");
    }
}
