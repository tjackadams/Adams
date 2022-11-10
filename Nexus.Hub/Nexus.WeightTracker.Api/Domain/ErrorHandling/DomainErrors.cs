using FluentResults;

namespace Nexus.WeightTracker.Api.Domain.ErrorHandling;

public class InvalidRecordedValue : Error
{
    public InvalidRecordedValue(decimal recordedValue)
        : base($"The provided value {recordedValue} is invalid.")
    {

    }
}
