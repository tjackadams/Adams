using StronglyTypedIds;

namespace Nexus.WeightTracker.Api.Domain;

[StronglyTypedId(StronglyTypedIdBackingType.Int,
    StronglyTypedIdConverter.TypeConverter | StronglyTypedIdConverter.SystemTextJson |
    StronglyTypedIdConverter.EfCoreValueConverter)]
public partial struct ClientId
{
    public static bool TryParse(string? value, IFormatProvider? _, out ClientId result)
    {
        if (!string.IsNullOrEmpty(value) && int.TryParse(value, out var clientId))
        {
            result = new ClientId(clientId);
            return true;
        }

        result = new ClientId();
        return false;
    }
}


public class Client
{
    public const int MaximumNameLength = 20;
    private Client()
    {

    }

    public ClientId ClientId { get; private set; }

    public string Name { get; private set; }

    public DateTimeOffset CreatedTime { get; private set; }

    public static Client Create(string name)
    {
        return new Client { Name = name };
    }
}
