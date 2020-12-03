using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adams.Services.Smoking.Api.Features.Recipes.Models;
using Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate;
using Adams.Services.Smoking.Infrastructure;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Adams.Services.Smoking.Api.Features.Recipes.Queries
{
    public class GetRecipe
    {
        public record Query : IRequest<RecipeModel>
        {
            [FromRoute]
            public string Name { get; init; }
        }

        public class Handler : IRequestHandler<Query, RecipeModel>
        {
            private readonly IConfigurationProvider _configuration;
            private readonly SmokingContext _db;

            public Handler(SmokingContext db, IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public Task<RecipeModel> Handle(Query request, CancellationToken cancellationToken)
            {
                return _db.Recipes
                    .Where(r => r.Name == request.Name)
                    .Include(r => r.Steps)
                    .ProjectTo<RecipeModel>(_configuration)
                    .SingleOrDefaultAsync(cancellationToken);
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Recipe, RecipeModel>();
                CreateMap<RecipeStep, RecipeStepModel>();
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
    }
}