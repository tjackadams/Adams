using System;
using Adams.Domain;

namespace Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate
{
    public class RecipeStep : Entity
    {
        public static double MinimumTemperature => 0;
        public static double MaximumTemperature => 500;
        public static TimeSpan MinimumDuration => TimeSpan.Zero;
        public static TimeSpan MaximumDuration => TimeSpan.FromHours(24);
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
        public TimeSpan? Duration { get; set; } = null;
        public double? Temperature { get; set; } = null;
    }
}