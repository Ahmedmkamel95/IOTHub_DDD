using System.Net.Http.Json;
using System.Text.Json;
using TUnit.Core;

namespace CIOT.Integration.Tests.Infrastructure;

internal static class ApiIntegrationAssertions
{
    internal static async Task<JsonElement> ReadJsonAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken
    )
    {
        return await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
    }

    internal static async Task AssertErrorCodeAsync(
        HttpResponseMessage response,
        string expectedCode,
        CancellationToken cancellationToken
    )
    {
        var document = await ReadJsonAsync(response, cancellationToken);
        await Assert.That(document.GetProperty("code").GetString()).IsEqualTo(expectedCode);
    }

    internal static Guid GetId(JsonElement document) => document.GetProperty("id").GetGuid();
}
