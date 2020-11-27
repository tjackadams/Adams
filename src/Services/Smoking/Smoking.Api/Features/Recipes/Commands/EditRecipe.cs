using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate;
using Adams.Services.Smoking.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Adams.Services.Smoking.Api.Features.Recipes.Commands
{
    public class EditRecipe
    {
        public record Command : IRequest
        {
            [FromRoute(Name = "name")]
            public string Name { get; init; }

            [FromBody]
            public CommandData Data { get; init; }

            public record CommandData
            {
                public string DisplayName { get; init; }
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
                RuleFor(p => p.Data).ChildRules(x =>
                {
                    x.RuleFor(p => p.DisplayName)
                        .NotEmpty()
                        .MaximumLength(200);

                    x.RuleFor(p => p.Description)
                        .NotEmpty()
                        .MaximumLength(2000);
                });
            }
        }

        private static class Mapper
        {
            public static void UpdateEntity(Command command, Recipe entity)
            {
                entity
                    .SetDisplayName(command.Data.DisplayName)
                    .SetDescription(command.Data.Description);
            }
        }
    }
}