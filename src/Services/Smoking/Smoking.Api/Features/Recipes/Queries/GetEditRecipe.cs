using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adams.Services.Smoking.Api.Features.Recipes.Commands;
using Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate;
using Adams.Services.Smoking.Infrastructure;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Adams.Services.Smoking.Api.Features.Recipes.Queries
{
    public class GetEditRecipe
    {
        public record Query : IRequest<EditRecipe.Command>
        {
            public string Name { get; init; }
        }

        public class Handler : IRequestHandler<Query, EditRecipe.Command>
        {
            private readonly IConfigurationProvider _configuration;
            private readonly SmokingContext _db;

            public Handler(SmokingContext db, IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public Task<EditRecipe.Command> Handle(Query request, CancellationToken cancellationToken)
            {
                return _db.Recipes
                    .Where(r => r.Name == request.Name)
                    .ProjectTo<EditRecipe.Command>(_configuration)
                    .SingleOrDefaultAsync(cancellationToken);
            }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(p => p.Name)
                    .NotEmpty();
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Recipe, EditRecipe.Command>();
                CreateMap<RecipeStep, EditRecipe.Command.CommandStep>();
            }
        }
    }
}