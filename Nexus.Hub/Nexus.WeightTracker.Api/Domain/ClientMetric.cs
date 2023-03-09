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
        ClientMetricId = ClientMetricId.Empty;
        ClientId = clientId;
        CreatedTime = DateTimeOffset.UtcNow;
        RecordedValue = recordedValue;
        RecordedDate = recordedDate;
    }


    public ClientMetricId ClientMetricId { get; init; }

    public ClientId ClientId { get; init; }

    public Client Client { get; init; } = null!;

    public double RecordedValue { get; init; }

    public DateOnly RecordedDate { get; init; }

    public DateTimeOffset CreatedTime { get; init; }
}
