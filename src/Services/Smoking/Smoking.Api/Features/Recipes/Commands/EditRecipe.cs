using System.Linq;
using System.Text.Json.Serialization;
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
                var recipe = await _db.Recipes.Where(r => r.Name == request.Name)
                    .FirstOrDefaultAsync(cancellationToken);

                Mapper.UpdateEntity(request, recipe);

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
            }
        }

        private static class Mapper
        {
            public static void UpdateEntity(Command command, Recipe entity)
            {
                entity
                    .SetDisplayName(command.DisplayName)
                    .SetDescription(command.Description);
            }
        }
    }
}