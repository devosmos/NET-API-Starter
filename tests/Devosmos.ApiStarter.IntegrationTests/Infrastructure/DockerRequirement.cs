namespace Devosmos.ApiStarter.IntegrationTests.Infrastructure;

public static class DockerRequirement
{
    public static bool IsAvailable
    {
        get
        {
            if (string.Equals(
                    Environment.GetEnvironmentVariable("DEVOSMOS_SKIP_DOCKER_TESTS"),
                    "true",
                    StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DOCKER_HOST")))
            {
                return true;
            }

            return OperatingSystem.IsWindows()
                ? File.Exists(@"\\.\pipe\docker_engine")
                : File.Exists("/var/run/docker.sock");
        }
    }
}
