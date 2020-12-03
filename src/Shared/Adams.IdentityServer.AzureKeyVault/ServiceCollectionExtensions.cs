using System;
using Adams.IdentityServer.AzureKeyVault;
using Adams.IdentityServer.AzureKeyVault.Stores;
using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using IdentityServer4.Stores;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IIdentityServerBuilder AddSigningCredentialsFromAzureKeyVault(this IIdentityServerBuilder builder,
            Action<IdentityServerAzureKeyVaultOptions> setupAction)
        {
            builder.Services.AddMemoryCache();

            builder.Services.Configure(setupAction);

            builder.Services.AddSingleton<ISigningCredentialStore, AzureKeyVaultSigningCredentialStore>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<IdentityServerAzureKeyVaultOptions>>().Value;
                return new AzureKeyVaultSigningCredentialStore(
                    sp.GetRequiredService<IMemoryCache>(),
                    new CertificateClient(
                        new Uri(options.VaultUri),
                        new DefaultAzureCredential()),
                    new SecretClient(
                        new Uri(options.VaultUri),
                        new DefaultAzureCredential()),
                    sp.GetRequiredService<IOptionsMonitor<IdentityServerAzureKeyVaultOptions>>()
                );
            });

            builder.Services.AddSingleton<IValidationKeysStore, AzureKeyVaultValidationKeysStore>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<IdentityServerAzureKeyVaultOptions>>().Value;
                return new AzureKeyVaultValidationKeysStore(
                    sp.GetRequiredService<IMemoryCache>(),
                    new CertificateClient(
                        new Uri(options.VaultUri),
                        new DefaultAzureCredential()),
                    new SecretClient(
                        new Uri(options.VaultUri),
                        new DefaultAzureCredential()),
                    sp.GetRequiredService<IOptionsMonitor<IdentityServerAzureKeyVaultOptions>>()
                );
            });

            return builder;
        }
    }
}