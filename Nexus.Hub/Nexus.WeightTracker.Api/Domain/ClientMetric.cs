using StronglyTypedIds;

namespace Nexus.WeightTracker.Api.Domain;


[StronglyTypedId(StronglyTypedIdBackingType.Int, StronglyTypedIdConverter.TypeConverter | StronglyTypedIdConverter.SystemTextJson | StronglyTypedIdConverter.EfCoreValueConverter)]
public partial struct ClientMetricId
{
    public static bool TryParse(string? value, IFormatProvider? _, out ClientMetricId result)
    {
        if (!string.IsNullOrEmpty(value) && int.TryParse(value, out var clientId))
        {
            result = new ClientMetricId(clientId);
            return true;
        }

        result = new ClientMetricId();
        return false;
    }
}

public class ClientMetric
{
    protected ClientMetric() { }
    public ClientMetric(ClientId clientId, double recordedValue, DateOnly recordedDate)
    {
        ClientId = clientId;
        RecordedValue = recordedValue;
        RecordedDate = recordedDate;
    }


    public ClientMetricId ClientMetricId { get; private set; }

    public ClientId ClientId { get; private set; }

    public Client Client {get; private set;}

    public double RecordedValue { get; private set; }

    public DateOnly RecordedDate { get; private set; }

    public DateTimeOffset CreatedTime { get; private set; }
}
