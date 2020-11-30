using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBlazor.Client.Models.Smoking
{
    public record RecipeSummary
    {
        public string Name { get; init; }
        public string DisplayName { get; init; }
        public int Steps { get; init; }
    }
}
