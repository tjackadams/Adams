using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using Microsoft.IdentityModel.Tokens;

namespace Adams.IdentityServer.AzureKeyVault.Stores
{
    public abstract class KeyStore
    {
        private readonly CertificateClient _certificateClient;
        private readonly SecretClient _secretClient;
        protected KeyStore(CertificateClient certificateClient, SecretClient secretClient)
        {
            _certificateClient = certificateClient;
            _secretClient = secretClient;
        }

        protected async Task<List<CertificateProperties>> GetAllEnabledCertificateVersionsAsync(string certificateName)
        {
            // get all certificate versions. this includes the current active version.
            var certificateVersions = _certificateClient.GetPropertiesOfCertificateVersionsAsync(certificateName);

            List<CertificateProperties> certificates = new List<CertificateProperties>();

            await foreach (var certificate in certificateVersions)
            {
                if (certificate.Enabled.HasValue && certificate.Enabled.Value)
                {
                    certificates.Add(certificate);
                }
            }

            return certificates.OrderByDescending(v => v.CreatedOn).ToList();
        }

        protected async Task<SigningCredentials> GetSigningCredentialsAsync(CertificateProperties certificate)
        {
            var securityKey = await GetSecurityKeyAsync(certificate);
            return new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
        }

        protected async Task<SecurityKey> GetSecurityKeyAsync(CertificateProperties certificate)
        {
            var secret = await _secretClient.GetSecretAsync(certificate.Name, certificate.Version);
            var privateKeyBytes = Convert.FromBase64String(secret.Value.Value);
            var certificateWithPrivateKey =
                new X509Certificate2(privateKeyBytes, (string) null, X509KeyStorageFlags.MachineKeySet);
            return new X509SecurityKey(certificateWithPrivateKey);
        }
    }
}
