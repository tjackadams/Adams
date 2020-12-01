using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Adams.Services.Smoking.Api.Features.Recipes.Commands;
using Adams.Services.Smoking.Api.Features.Recipes.Models;
using Adams.Services.Smoking.Api.Features.Recipes.Queries;
using HybridModelBinding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adams.Services.Smoking.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecipesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<RecipeSummary>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecipes([FromQuery] GetRecipes.Query query,
            CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipe.Command command,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("{name}/edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEditRecipe([FromRoute] GetEditRecipe.Query query,
            CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpPost("{name}/edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditRecipe([FromHybrid] EditRecipe.Command command,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}