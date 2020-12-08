using System.Collections.Generic;
using Adams.Domain;

namespace Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate
{
    public class Recipe : Entity, IAggregateRoot
    {
        public List<RecipeStep> Steps { get; set; } = new List<RecipeStep>();
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Protein Protein { get; set; }
        public int ProteinId { get; set; }
    }
}