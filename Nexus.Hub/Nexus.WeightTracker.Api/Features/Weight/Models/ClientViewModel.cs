using AutoMapper;
using Nexus.WeightTracker.Api.Domain;

namespace Nexus.WeightTracker.Api.Features.Weight.Models;

public record struct ClientViewModel(ClientId ClientId, string Name);

public class ClientViewModelProfile : Profile
{
    public ClientViewModelProfile()
    {
        CreateMap<Client, ClientViewModel>()
            .ForCtorParam(nameof(ClientViewModel.ClientId), opt => opt.MapFrom(c => c.ClientId))
            .ForCtorParam(nameof(ClientViewModel.Name), opt => opt.MapFrom(c => c.Name));
    }
}
