using Microsoft.AspNetCore.Mvc;

namespace Adams.Services.Smoking.Api.Features.Recipes.Queries
{
    public class GetRecipe
    {
        public class Query
        {
            [FromRoute]
            public string Name { get; set; }
        }
    }
}