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

        private int _step;
        private string _description;
        protected RecipeStep(){}
        public RecipeStep(int recipeId, int step, string description)
        {
            RecipeId = recipeId;

            _step = step;
            _description = description;
        }
    }
}
