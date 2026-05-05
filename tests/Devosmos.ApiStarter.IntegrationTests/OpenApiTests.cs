using Devosmos.ApiStarter.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace Devosmos.ApiStarter.IntegrationTests;

[Collection(ApiTestCollection.Name)]
public sealed class OpenApiTests(ApiTestFixture fixture)
{
    [Fact]
    public async Task OpenApi_Document_Should_Be_Available_In_Testing()
    {
        var factory = fixture.GetFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/openapi/v1.json", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Contain("\"openapi\"");
    }
}
