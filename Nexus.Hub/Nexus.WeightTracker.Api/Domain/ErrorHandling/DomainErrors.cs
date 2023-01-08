using FluentResults;

namespace Nexus.WeightTracker.Api.Domain.ErrorHandling;

public class InvalidRecordedValue : Error
{
    public InvalidRecordedValue(double recordedValue)
        : base($"The provided value {recordedValue} is invalid.")
    {
        Metadata.Add("PropertyName", "RecordedValue");
    }
}

public class DuplicateRecordedDate : Error
{
    public DuplicateRecordedDate(DateOnly recordedDate)
    : base($"The date {recordedDate} has already been recorded.")
    {
        Metadata.Add("PropertyName", "RecordedDate");
    }
}

