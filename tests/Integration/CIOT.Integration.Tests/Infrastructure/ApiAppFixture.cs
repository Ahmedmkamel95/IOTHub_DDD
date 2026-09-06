using TUnit.Aspire;

namespace CIOT.Integration.Tests.Infrastructure;

public sealed class ApiAppFixture : AspireFixture<Projects.CIOT_AppHost>
{
    public const string ApiResourceName = "ciot-api";

    protected override TimeSpan ResourceTimeout => TimeSpan.FromMinutes(3);

    protected override ResourceWaitBehavior WaitBehavior => ResourceWaitBehavior.Named;

    protected override IEnumerable<string> ResourcesToWaitFor() => [ApiResourceName];

    protected override IEnumerable<string> ResourcesToRemove() => ["postgres-mcp", "pgadmin"];

    public HttpClient CreateApiClient() => CreateHttpClient(ApiResourceName, "http");
}
