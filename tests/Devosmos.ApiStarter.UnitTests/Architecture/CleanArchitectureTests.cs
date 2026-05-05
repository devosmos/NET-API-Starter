using System.Xml.Linq;
using FluentAssertions;

namespace Devosmos.ApiStarter.UnitTests.Architecture;

public sealed class CleanArchitectureTests
{
    [Theory]
    [InlineData("src/Devosmos.ApiStarter.Domain/Devosmos.ApiStarter.Domain.csproj")]
    public void Domain_Should_Not_Reference_Other_Projects(string projectPath)
    {
        var references = ReadProjectReferences(projectPath);

        references.Should().BeEmpty();
    }

    [Theory]
    [InlineData(
        "src/Devosmos.ApiStarter.Application/Devosmos.ApiStarter.Application.csproj",
        "src/Devosmos.ApiStarter.Domain/Devosmos.ApiStarter.Domain.csproj")]
    public void Application_Should_Reference_Only_Domain(string projectPath, string expectedReference)
    {
        var references = ReadProjectReferences(projectPath);

        references.Should().BeEquivalentTo([NormalizePath(expectedReference)]);
    }

    [Theory]
    [InlineData(
        "src/Devosmos.ApiStarter.Infrastructure/Devosmos.ApiStarter.Infrastructure.csproj",
        "src/Devosmos.ApiStarter.Application/Devosmos.ApiStarter.Application.csproj",
        "src/Devosmos.ApiStarter.Domain/Devosmos.ApiStarter.Domain.csproj")]
    public void Infrastructure_Should_Reference_Only_Application_And_Domain(
        string projectPath,
        string applicationReference,
        string domainReference)
    {
        var references = ReadProjectReferences(projectPath);

        references.Should().BeEquivalentTo([
            NormalizePath(applicationReference),
            NormalizePath(domainReference)
        ]);
    }

    [Theory]
    [InlineData(
        "src/Devosmos.ApiStarter.Api/Devosmos.ApiStarter.Api.csproj",
        "src/Devosmos.ApiStarter.Application/Devosmos.ApiStarter.Application.csproj",
        "src/Devosmos.ApiStarter.Infrastructure/Devosmos.ApiStarter.Infrastructure.csproj")]
    public void Api_Should_Reference_Application_And_Infrastructure(
        string projectPath,
        string applicationReference,
        string infrastructureReference)
    {
        var references = ReadProjectReferences(projectPath);

        references.Should().BeEquivalentTo([
            NormalizePath(applicationReference),
            NormalizePath(infrastructureReference)
        ]);
    }

    private static IReadOnlyCollection<string> ReadProjectReferences(string projectPath)
    {
        var root = FindRepositoryRoot();
        var projectFile = Path.Combine(root, NormalizePath(projectPath));
        var projectDirectory = Path.GetDirectoryName(projectFile)
            ?? throw new InvalidOperationException($"Project path has no directory: {projectFile}");

        var document = XDocument.Load(projectFile);

        return document
            .Descendants("ProjectReference")
            .Select(reference => reference.Attribute("Include")?.Value)
            .Where(reference => !string.IsNullOrWhiteSpace(reference))
            .Select(reference => Path.GetFullPath(Path.Combine(projectDirectory, reference!)))
            .Select(reference => Path.GetRelativePath(root, reference))
            .Select(NormalizePath)
            .ToArray();
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Devosmos.ApiStarter.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new InvalidOperationException("Could not find repository root.");
    }

    private static string NormalizePath(string path)
    {
        return path.Replace('\\', '/');
    }
}
