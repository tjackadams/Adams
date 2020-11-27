using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adams.Domain;

namespace Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate
{
    public class RecipeStep : Entity
    {
        public int RecipeId { get; private set; }
        protected RecipeStep(){}
        public RecipeStep(int recipeId, int step, string description)
        {
            RecipeId = recipeId;

            Step = step;
            Description = description;
        }

        public int Step {get;private set;}
        public string Description { get; private set; }
    }
}
