using System.Collections.Generic;
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
    public class GetProteins
    {
        public record Query : IRequest<List<ProteinModel>>
        {
        }

        public class Handler : IRequestHandler<Query, List<ProteinModel>>
        {
            private readonly IConfigurationProvider _configuration;
            private readonly SmokingContext _db;

            public Handler(SmokingContext db, IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public Task<List<ProteinModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                return _db.Proteins
                    .ProjectTo<ProteinModel>(_configuration)
                    .ToListAsync(cancellationToken);
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Protein, ProteinModel>();
            }
        }
    }
}