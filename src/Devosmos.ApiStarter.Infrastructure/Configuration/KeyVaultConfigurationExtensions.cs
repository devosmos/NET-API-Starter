using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace Devosmos.ApiStarter.Infrastructure.Configuration;

public static class KeyVaultConfigurationExtensions
{
    public static IConfigurationBuilder AddConfiguredKeyVault(this IConfigurationBuilder builder)
    {
        var configuration = builder.Build();
        var vaultUri = configuration["KeyVault:Uri"] ?? configuration["KeyVault:Endpoint"];

        if (string.IsNullOrWhiteSpace(vaultUri))
        {
            return builder;
        }

        builder.AddAzureKeyVault(new Uri(vaultUri), new DefaultAzureCredential());
        return builder;
    }
}
