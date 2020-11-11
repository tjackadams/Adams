using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Adams.IdentityServer.AzureKeyVault.Stores
{
    public class AzureKeyVaultValidationKeysStore : KeyStore, IValidationKeysStore
    {
        private readonly IMemoryCache _cache;
        private readonly IOptionsMonitor<IdentityServerAzureKeyVaultOptions> _options;

        public AzureKeyVaultValidationKeysStore(IMemoryCache cache, CertificateClient certificateClient,
            SecretClient secretClient, IOptionsMonitor<IdentityServerAzureKeyVaultOptions> options)
            : base(certificateClient, secretClient)
        {
            _cache = cache;
            _options = options;
        }

        public async Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync()
        {
            if (_cache.TryGetValue(CacheKey.ValidationKeys, out List<SecurityKeyInfo> validationKeys))
            {
                return validationKeys;
            }

            validationKeys = new List<SecurityKeyInfo>();

            var enabledCertificateVersions =
                await GetAllEnabledCertificateVersionsAsync(_options.CurrentValue.CertificateName);
            foreach (var cert in enabledCertificateVersions)
            {
                var key = await GetSecurityKeyAsync(cert);
                validationKeys.Add(new SecurityKeyInfo {Key = key, SigningAlgorithm = SecurityAlgorithms.RsaSha256});
            }

            _cache.Set(CacheKey.ValidationKeys, validationKeys,
                DateTimeOffset.UtcNow.Add(_options.CurrentValue.CacheExpiration));

            return validationKeys;
        }
    }
}