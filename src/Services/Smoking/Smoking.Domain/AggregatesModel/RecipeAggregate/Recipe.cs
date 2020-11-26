using System;
using System.Collections.Generic;
using Adams.Domain;

namespace Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate
{
    public class Recipe : Entity, IAggregateRoot
    {
        private readonly List<RecipeStep> _steps;
        public IReadOnlyCollection<RecipeStep> Steps => _steps;

        private string _name;
        private string _displayName;
        private string _description;

        protected Recipe()
        {
            _steps = new List<RecipeStep>();
        }

        public Recipe(string name, string displayName, string description) 
            : this()
        {
            _name = name;
            _displayName = displayName;
            _description = description;
        }
    }
}