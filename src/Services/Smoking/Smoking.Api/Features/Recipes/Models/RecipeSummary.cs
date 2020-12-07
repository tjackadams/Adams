namespace Adams.Services.Smoking.Api.Features.Recipes.Models
{
    public record RecipeSummary
    {
        public string Name { get; init; }
        public string DisplayName { get; init; }
        public string Protein { get; init; }
        public int Steps { get; init; }
    }
}