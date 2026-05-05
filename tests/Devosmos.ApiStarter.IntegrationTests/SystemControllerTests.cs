using System.Net.Http.Headers;
using System.Net.Http.Json;
using Devosmos.ApiStarter.Application.System;
using Devosmos.ApiStarter.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace Devosmos.ApiStarter.IntegrationTests;

[Collection(ApiTestCollection.Name)]
public sealed class SystemControllerTests(ApiTestFixture fixture)
{
    [Fact]
    public async Task GetInfo_Should_Return_Unauthorized_Without_Token()
    {
        var factory = fixture.GetFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/system/info", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetInfo_Should_Return_Metadata_For_Authenticated_User()
    {
        var factory = fixture.GetFactory();
        await factory.ResetDatabaseAsync();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.SchemeName);

        var response = await client.GetAsync("/api/v1/system/info", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadFromJsonAsync<ApiInfoResponse>(
            cancellationToken: TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().NotBeNull();
        content!.Name.Should().Be("Devosmos.ApiStarter");
        content.Runtime.Should().Be("net10.0");
    }
}
