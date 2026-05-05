using DotNet.Testcontainers.Builders;
using Xunit.Sdk;

namespace Devosmos.ApiStarter.IntegrationTests.Infrastructure;

public sealed class ApiTestFixture : IAsyncLifetime
{
    private const string DockerRequiredMessage =
        "Docker is required for SQL Server Testcontainers integration tests.";

    private string? _skipReason;

    public ApiTestFactory? Factory { get; private set; }

    public async ValueTask InitializeAsync()
    {
        if (!DockerRequirement.IsAvailable)
        {
            _skipReason = DockerRequiredMessage;
            return;
        }

        try
        {
            Factory = new ApiTestFactory();
            await Factory.InitializeAsync();
        }
        catch (DockerUnavailableException exception)
        {
            if (Factory is not null)
            {
                await Factory.DisposeAsync();
            }

            Factory = null;
            _skipReason = $"{DockerRequiredMessage} {exception.Message}";
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (Factory is not null)
        {
            await Factory.DisposeAsync();
        }
    }

    public ApiTestFactory GetFactory()
    {
        return Factory ?? throw SkipException.ForSkip(_skipReason ?? DockerRequiredMessage);
    }
}
