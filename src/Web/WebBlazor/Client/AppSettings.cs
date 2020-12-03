using System.Collections.Generic;

namespace WebBlazor.Client
{
    public record AppSettings
    {
        public SmokerSettings Smoker { get; init; }
    }

    public record SmokerSettings
    {
        public string BaseAddress { get; init; }
        public List<string> AuthorizedUrls { get; init; }
        public List<string> Scopes { get; init; }
    }
}