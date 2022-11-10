using Nexus.AspNetCore.Helpers;
using NJsonSchema.Generation;

namespace Nexus.WeightTracker.Api.Infrastructure.NSwag;

public class NexusSchemaNameGenerator : ISchemaNameGenerator
{
    public string Generate(Type type)
    {
        return TypeNameHelper.GetTypeDisplayName(type, true, true, true, '_');
    }
}
