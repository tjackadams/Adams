using System.Collections.Generic;
using System.Linq;
using Adams.Domain;

namespace Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate
{
    public sealed class Recipe : Entity, IAggregateRoot
    {
        private readonly List<RecipeStep> _steps;

        private Recipe()
        {
            _steps = new List<RecipeStep>
            {
                RecipeStep.Start,
                RecipeStep.Finish
            };
        }

        public Recipe(string name, string displayName, string description)
            : this()
        {
            Name = name;
            DisplayName = displayName;
            Description = description;
        }

        public IReadOnlyCollection<RecipeStep> Steps => _steps;

        public string Name { get; }

        public string DisplayName { get; private set; }

        public string Description { get; private set; }

        public void AddRecipeStep(RecipeStep step)
        {
            if (!step.IsTransient())
            {
                var existingStep = _steps.SingleOrDefault(s => s.Id == step.Id);

                if (existingStep != null)
                {
                    existingStep.Description = step.Description;
                    existingStep.Step = step.Step;

                    return;
                }
            }

            _steps.Add(step);
        }

        public void RemoveRecipeStep(RecipeStep step)
        {
            _steps.Remove(step);
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