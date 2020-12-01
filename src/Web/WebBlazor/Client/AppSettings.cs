using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public List<string> Scopes { get; init;  }
    }
}
