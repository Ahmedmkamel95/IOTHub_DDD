using System.Net;
using System.Net.Http.Json;
using CIOT.Integration.Tests.Infrastructure;
using TUnit.Core;

namespace CIOT.Integration.Tests.Identity;

[ClassDataSource<ApiAppFixture>(Shared = SharedType.PerTestSession)]
public sealed class IdentityEndpointsTests(ApiAppFixture fixture)
{
    [Test]
    public async Task CreateUser_ThenGetById_ReturnsPersistedUser(
        CancellationToken cancellationToken
    )
    {
        var client = fixture.CreateApiClient();
        var email = $"integration-{Guid.NewGuid():N}@example.test";
        var createResponse = await client.PostAsJsonAsync(
            "/api/identity/users",
            new
            {
                email,
                displayName = "Integration User",
                firstName = "Integration",
                lastName = "User",
                userType = "External",
                mainCountryCode = "EG",
                roleNames = (string[]?)null
            },
            cancellationToken
        );

        await Assert.That(createResponse.StatusCode).IsEqualTo(HttpStatusCode.Created);
        var created = await ApiIntegrationAssertions.ReadJsonAsync(createResponse, cancellationToken);
        var userId = ApiIntegrationAssertions.GetId(created);
        await Assert.That(created.GetProperty("email").GetString()).IsEqualTo(email.ToLowerInvariant());
        await Assert.That(created.GetProperty("status").GetString()).IsEqualTo("Invited");
        await Assert.That(created.GetProperty("roles").GetArrayLength()).IsEqualTo(0);

        var getResponse = await client.GetAsync($"/api/identity/users/{userId}", cancellationToken);
        await Assert.That(getResponse.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var document = await ApiIntegrationAssertions.ReadJsonAsync(getResponse, cancellationToken);
        await Assert.That(document.GetProperty("id").GetGuid()).IsEqualTo(userId);
        await Assert.That(document.GetProperty("userType").GetString()).IsEqualTo("External");
    }

    [Test]
    public async Task DuplicateEmail_ReturnsConflictError(CancellationToken cancellationToken)
    {
        var client = fixture.CreateApiClient();
        var request = new
        {
            email = $"duplicate-{Guid.NewGuid():N}@example.test",
            displayName = "Duplicate User",
            firstName = "Duplicate",
            lastName = "User",
            userType = "Internal",
            mainCountryCode = "EG",
            roleNames = (string[]?)null
        };

        var first = await client.PostAsJsonAsync("/api/identity/users", request, cancellationToken);
        await Assert.That(first.StatusCode).IsEqualTo(HttpStatusCode.Created);

        var duplicate = await client.PostAsJsonAsync(
            "/api/identity/users",
            request,
            cancellationToken
        );
        await Assert.That(duplicate.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        await ApiIntegrationAssertions.AssertErrorCodeAsync(
            duplicate,
            "User.DuplicateEmail",
            cancellationToken
        );
    }

    [Test]
    public async Task InvalidUserAndUnknownRoleRequests_ReturnDomainErrors(
        CancellationToken cancellationToken
    )
    {
        var client = fixture.CreateApiClient();
        var invalidUserResponse = await client.PostAsJsonAsync(
            "/api/identity/users",
            new
            {
                email = "not-an-email",
                displayName = "Invalid User",
                firstName = (string?)null,
                lastName = (string?)null,
                userType = "Unknown",
                mainCountryCode = (string?)null,
                roleNames = (string[]?)null
            },
            cancellationToken
        );
        await Assert.That(invalidUserResponse.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        var unknownUserResponse = await client.GetAsync(
            $"/api/identity/users/{Guid.NewGuid()}",
            cancellationToken
        );
        await Assert.That(unknownUserResponse.StatusCode).IsEqualTo(HttpStatusCode.NotFound);

        var unknownRoleResponse = await client.PostAsJsonAsync(
            $"/api/identity/users/{Guid.NewGuid()}/roles",
            new { roleName = "Role.Does.Not.Exist" },
            cancellationToken
        );
        await Assert.That(unknownRoleResponse.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        await ApiIntegrationAssertions.AssertErrorCodeAsync(
            unknownRoleResponse,
            "User.NotFound",
            cancellationToken
        );
    }

    [Test]
    public async Task RolesEndpoint_ReturnsJsonCollection(CancellationToken cancellationToken)
    {
        var response = await fixture.CreateApiClient().GetAsync(
            "/api/identity/roles",
            cancellationToken
        );

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var roles = await ApiIntegrationAssertions.ReadJsonAsync(response, cancellationToken);
        await Assert.That(roles.ValueKind).IsEqualTo(System.Text.Json.JsonValueKind.Array);
    }
}
