using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adams.Services.Smoking.Api.Features.Recipes.Models;
using Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate;
using Adams.Services.Smoking.Infrastructure;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Adams.Services.Smoking.Api.Features.Recipes.Queries
{
    public class GetRecipes
    {
        public record Query : IRequest<List<RecipeSummary>>
        {
            public string Search { get; init; }
        }

        public class Handler : IRequestHandler<Query, List<RecipeSummary>>
        {
            private readonly SmokingContext _db;
            private readonly IMapper _mapper;

            public Handler(SmokingContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public Task<List<RecipeSummary>> Handle(Query request, CancellationToken cancellationToken)
            {
                return _db.Recipes
                    .Where(r => string.IsNullOrEmpty(request.Search) || r.Name.Contains(request.Search) ||
                                r.DisplayName.Contains(request.Search))
                    .ProjectTo<RecipeSummary>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Recipe, RecipeSummary>()
                    .ForMember(dest => dest.Steps, opt => opt.MapFrom(src => src.Steps.Count()));
            }
        }
    }
}