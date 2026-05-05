using Devosmos.ApiStarter.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace Devosmos.ApiStarter.IntegrationTests;

[Collection(ApiTestCollection.Name)]
public sealed class HealthTests(ApiTestFixture fixture)
{
    [Fact]
    public async Task Live_Health_Should_Return_Ok()
    {
        var factory = fixture.GetFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/health/live", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Ready_Health_Should_Return_Ok_When_Database_Is_Available()
    {
        var factory = fixture.GetFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/health/ready", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
