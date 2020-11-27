using System;
using System.Collections.Generic;
using Adams.Domain;

namespace Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate
{
    public sealed class Recipe : Entity, IAggregateRoot
    {
        private readonly List<RecipeStep> _steps;
        public IReadOnlyCollection<RecipeStep> Steps => _steps;

        private Recipe()
        {
            _steps = new List<RecipeStep>
            {
                new (Id, 1, "Start"),
                new (Id, 2, "Finish")
            };
        }

        public Recipe(string name, string displayName, string description) 
            : this()
        {
            Name = name;
            DisplayName = displayName;
            Description = description;
        }

        public string Name { get; private set; }

        public string DisplayName { get; private set; }

        public string Description { get; private set; }

        public void AddRecipeStep(int step, string description)
        {
            var recipeStep = new RecipeStep(Id, step, description);
            _steps.Add(recipeStep);
        }

        public Recipe SetDisplayName(string displayName)
        {
            DisplayName = displayName;
            return this;
        }

        public Recipe SetDescription(string description)
        {
            Description = description;
            return this;
        }
    }
}