using System.Collections.Generic;

namespace WebBlazor.Client
{
    public record AppSettings
    {
        public ServicesSettings Services { get; init; }
    }

    public record ServicesSettings
    {
        public string BaseAddress { get; init; }
        public List<string> AuthorizedUrls { get; init; }
        public List<string> Scopes { get; init; }
    }
}