using NJsonSchema;

namespace Nexus.Todo.Api.Infrastructure.NSwag;

public class NexusTypeNameGenerator : ITypeNameGenerator
{
    public string Generate(JsonSchema schema, string typeNameHint, IEnumerable<string> reservedTypeNames)
    {
        return typeNameHint;
    }
}
