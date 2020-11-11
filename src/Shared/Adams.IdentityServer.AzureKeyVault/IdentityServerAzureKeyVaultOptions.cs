using System;

namespace Adams.IdentityServer.AzureKeyVault
{
    public class IdentityServerAzureKeyVaultOptions
    {
        public string VaultUri { get;set; }
        public string CertificateName { get; set;  }
        public TimeSpan RolloverTime { get; set; } = new TimeSpan(24, 0, 0);
        public TimeSpan CacheExpiration { get; set; } = new TimeSpan(24, 0, 0);

    }
}
