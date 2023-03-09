using Nexus.WeightTracker.Api.Domain.ErrorHandling;
using StronglyTypedIds;

namespace Nexus.WeightTracker.Api.Domain;

[StronglyTypedId(StronglyTypedIdBackingType.Int, StronglyTypedIdConverter.TypeConverter | StronglyTypedIdConverter.SystemTextJson | StronglyTypedIdConverter.EfCoreValueConverter)]
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
    private readonly List<ClientMetric> _metrics = new();

    public const int MaximumNameLength = 20;
    protected Client() { }

    public Client(string name, string createdBy)
    {
        ClientId = ClientId.Empty;
        CreatedBy = createdBy;
        CreatedTime = DateTimeOffset.UtcNow;
        Name = name;
        Version = Array.Empty<byte>();
    }

    public ClientId ClientId { get; private set; }

    public string Name { get; private set; } = null!;

    public DateTimeOffset CreatedTime { get; private set; }

    public string CreatedBy { get; private set; } = null!;

    public IReadOnlyCollection<ClientMetric> Metrics => _metrics.AsReadOnly();

    public byte[] Version { get; private set; } = null!;

    public FluentResults.Result AddMetric(double recordedValue, DateOnly recordedDate)
    {
        var result = FluentResults.Result.Ok();

        if (recordedValue <= 0)
        {
            result.WithError(DomainErrorProvider.InvalidRecordedValue(recordedValue));
        }

        if (_metrics.Any(m => m.RecordedDate == recordedDate))
        {

        }

        _metrics.Add(new ClientMetric(ClientId, recordedValue, recordedDate));

        return result;
    }
}
