using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adams.Services.Smoking.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Adams.Services.Smoking.Api.Features.Recipes.Commands
{
    public class DeleteRecipe
    {
        public record Command : IRequest
        {
            [FromRoute]
            public string Name { get; init; }
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
                var recipe = await _db.Recipes
                    .Where(r => r.Name == request.Name)
                    .SingleOrDefaultAsync(cancellationToken);

                if (recipe == null)
                {
                    return Unit.Value;
                }

                _db.Remove(recipe);
                return Unit.Value;
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(p => p.Name)
                    .NotEmpty();
            }
        }
    }
}