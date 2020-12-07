using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adams.Services.Smoking.Api.Features.Recipes.Models
{
    public record ProteinModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
}
