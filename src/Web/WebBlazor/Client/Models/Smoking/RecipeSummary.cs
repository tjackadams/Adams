namespace WebBlazor.Client.Models.Smoking
{
    public record RecipeSummary
    {
        public string Name { get; init; }
        public string DisplayName { get; init; }
        public int Steps { get; init; }
    }
}