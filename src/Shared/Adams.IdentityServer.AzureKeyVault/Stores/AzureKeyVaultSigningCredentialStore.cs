using System;
using System.Linq;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using IdentityServer4.Stores;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Adams.IdentityServer.AzureKeyVault.Stores
{
    public class AzureKeyVaultSigningCredentialStore : KeyStore, ISigningCredentialStore
    {
        private readonly IMemoryCache _cache;
        private readonly IOptionsMonitor<IdentityServerAzureKeyVaultOptions> _options;

        public AzureKeyVaultSigningCredentialStore(IMemoryCache cache, CertificateClient certificateClient,
            SecretClient secretClient, IOptionsMonitor<IdentityServerAzureKeyVaultOptions> options)
            : base(certificateClient, secretClient)
        {
            _cache = cache;
            _options = options;
        }

        public async Task<SigningCredentials> GetSigningCredentialsAsync()
        {
            if (_cache.TryGetValue(CacheKey.SigningCredentials, out SigningCredentials signingCredentials))
            {
                return signingCredentials;
            }

            signingCredentials = await GetValidSigningCredentialsAsync();

            if (signingCredentials == null)
            {
                return null;
            }

            _cache.Set(CacheKey.SigningCredentials, signingCredentials,
                DateTimeOffset.UtcNow.Add(_options.CurrentValue.CacheExpiration));

            return signingCredentials;
        }

        private async Task<SigningCredentials> GetValidSigningCredentialsAsync()
        {
            var enabledCertificateVersions =
                await GetAllEnabledCertificateVersionsAsync(_options.CurrentValue.CertificateName);

            if (!enabledCertificateVersions.Any())
            {
                return null;
            }

            // find the certificate that has a passed rollover time
            var certificateVersionWithPassedRolloverTime = enabledCertificateVersions
                .FirstOrDefault(v =>
                    v.CreatedOn.HasValue && v.CreatedOn.Value <
                    DateTimeOffset.UtcNow.Add(-_options.CurrentValue.RolloverTime)
                );

            // if no certificate is found, it could indicate a newly created certificate.
            if (certificateVersionWithPassedRolloverTime == null)
            {
                return await GetSigningCredentialsAsync(enabledCertificateVersions.First());
            }

            return await GetSigningCredentialsAsync(certificateVersionWithPassedRolloverTime);
        }
    }
}