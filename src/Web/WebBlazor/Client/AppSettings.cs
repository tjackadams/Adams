using System.Collections.Generic;

namespace WebBlazor.Client
{
    public record AppSettings
    {
        public SmokingSettings Smoking { get; init; }
        public List<string> AuthorizedUrls { get; init; }
        public List<string> Scopes { get; init; }
    }

    public record SmokingSettings
    {
        public string BaseAddress { get; init; }

    }
}