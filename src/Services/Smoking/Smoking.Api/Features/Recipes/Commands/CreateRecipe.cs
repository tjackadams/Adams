using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate;
using Adams.Services.Smoking.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Adams.Services.Smoking.Api.Features.Recipes.Commands
{
    public class CreateRecipe
    {
        public record Command : IRequest<Unit>
        {
            public string Name { get; init; }
            public string DisplayName { get; init; }
            public string Description { get; init; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly SmokingContext _db;

            public Handler(SmokingContext db)
            {
                _db = db;
            }

            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = Mapper.ToEntity(request);

                _db.Attach(entity);

                return Unit.Task;
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(SmokingContext db)
            {
                RuleFor(p => p.Name)
                    .NotEmpty()
                    .MaximumLength(20)
                    .MustAsync(async (name, token) =>
                    {
                        return !await db.Recipes.Where(r => r.Name == name).AnyAsync(token);
                    })
                    .WithMessage(cmd => $"The name \"{cmd.Name}\" is already in use.");

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
            public static Recipe ToEntity(Command model)
            {
                return new(model.Name, model.DisplayName, model.Description);
            }
        }
    }
}