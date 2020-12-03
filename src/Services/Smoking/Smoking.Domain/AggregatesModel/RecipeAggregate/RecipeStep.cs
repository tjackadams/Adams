using Adams.Domain;

namespace Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate
{
    public class RecipeStep : Entity
    {
        public static RecipeStep Start => new()
        {
            Step = 1,
            Description = "Start"
        };

        public static RecipeStep Finish => new()
        {
            Step = 2,
            Description = "Finish"
        };

        public int RecipeId { get; set; }

        public int Step { get; set; }
        public string Description { get; set; }
    }
}