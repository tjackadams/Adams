using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adams.Services.Smoking.Api.Infrastructure.Filters;
using Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate;
using Adams.Services.Smoking.Infrastructure;
using FluentValidation;
using HybridModelBinding;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Adams.Services.Smoking.Api.Features.Recipes.Commands
{
    public class EditRecipe
    {
        public record Command : IRequest
        {
            [HybridBindProperty(Source.Route)]
            [SwaggerIgnore]
            public string Name { get; init; }

            [HybridBindProperty(Source.Body)]
            public string DisplayName { get; init; }

            [HybridBindProperty(Source.Body)]
            public string Description { get; init; }

            [HybridBindProperty(Source.Body)]
            public int ProteinId { get; init;  }

            [HybridBindProperty(Source.Body)]
            public List<CommandStep> Steps { get; init; }

            public record CommandStep
            {
                public int Id { get; init; }
                public int Step { get; init; }
                public string Description { get; init; }
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly SmokingContext _db;

            public Handler(SmokingContext db)
            {
                _db = db;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var protein = await _db.Proteins.Where(p => p.Id == request.ProteinId).SingleAsync(cancellationToken);

                var recipe = await _db.Recipes.Where(r => r.Name == request.Name)
                    .Include(r => r.Protein)
                    .Include(r => r.Steps)
                    .SingleOrDefaultAsync(cancellationToken);

                recipe
                    .SetDisplayName(request.DisplayName)
                    .SetDescription(request.Description)
                    .SetProtein(protein);

                var steps = request.Steps.Select(s => new RecipeStep
                    {Description = s.Description, Id = s.Id, Step = s.Step, RecipeId = recipe.Id}).ToList();

                // remove missing steps
                var staleSteps = recipe.Steps.Except(steps).ToList();
                foreach (var staleStep in staleSteps)
                {
                    recipe.RemoveRecipeStep(staleStep);
                }

                // add or update steps
                foreach (var step in steps)
                {
                    recipe.AddRecipeStep(step);
                }

                return Unit.Value;
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(p => p.DisplayName)
                    .NotEmpty()
                    .MaximumLength(200);

                RuleFor(p => p.Description)
                    .NotEmpty()
                    .MaximumLength(2000);

                RuleFor(p => p.ProteinId)
                    .NotEmpty()
                    .Must(x => Protein.List().Select(y => y.Id).Contains(x));

                RuleFor(p => p.Steps)
                    .Must(steps => steps.Count >= 2)
                    .WithMessage("Have you deleted the start and finish steps?")
                    .Must(steps => steps.Select(s => s.Step).SequenceEqual(Enumerable.Range(1, steps.Count)))
                    .WithMessage("Steps must be in sequential order.");

                RuleForEach(p => p.Steps).ChildRules(x =>
                {
                    x.RuleFor(p => p.Description)
                        .NotEmpty()
                        .MaximumLength(2000);

                    x.RuleFor(p => p.Step)
                        .NotEmpty();
                });
            }
        }
    }
}