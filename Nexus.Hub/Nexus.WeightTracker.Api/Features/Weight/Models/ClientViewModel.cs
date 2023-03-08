using AutoMapper;
using Nexus.WeightTracker.Api.Domain;

namespace Nexus.WeightTracker.Api.Features.Weight.Models;

public record ClientViewModel(ClientId Id, string Name);

public class ClientViewModelProfile : Profile
{
    public ClientViewModelProfile()
    {
        CreateMap<Client, ClientViewModel>()
            .ForCtorParam(nameof(ClientViewModel.Id), opt => opt.MapFrom(c => c.ClientId))
            .ForCtorParam(nameof(ClientViewModel.Name), opt => opt.MapFrom(c => c.Name));
    }
}
