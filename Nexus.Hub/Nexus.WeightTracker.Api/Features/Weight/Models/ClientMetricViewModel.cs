using AutoMapper;
using Nexus.WeightTracker.Api.Domain;

namespace Nexus.WeightTracker.Api.Features.Weight.Models;

public record struct ClientMetricViewModel(ClientMetricId ClientMetricId, double RecordedValueMetric,
    DateOnly RecordedDate);

public class ClientMetricViewModelProfile : Profile
{
    public ClientMetricViewModelProfile()
    {
        CreateMap<ClientMetric, ClientMetricViewModel>()
            .ForCtorParam(nameof(ClientMetricViewModel.ClientMetricId), opt => opt.MapFrom(c => c.ClientMetricId))
            .ForCtorParam(nameof(ClientMetricViewModel.RecordedValueMetric), opt => opt.MapFrom(c => c.RecordedValue))
            .ForCtorParam(nameof(ClientMetricViewModel.RecordedDate), opt => opt.MapFrom(c => c.RecordedDate));
    }
}
