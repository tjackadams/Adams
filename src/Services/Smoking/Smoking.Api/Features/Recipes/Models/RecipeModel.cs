using System.Collections.Generic;

namespace Adams.Services.Smoking.Api.Features.Recipes.Models
{
    public record RecipeModel
    {
        public string DisplayName { get; init; }
        public string Description { get; init; }
        public string Protein { get; init; }
        public List<RecipeStepModel> Steps { get; init; }
    }

    public record RecipeStepModel
    {
        public int Step { get; init; }
        public string Description { get; init; }
    }
}