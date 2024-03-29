﻿using FluentResults;

namespace Nexus.WeightTracker.Api.Domain.ErrorHandling;

public static class DomainErrorProvider
{
    public static Error InvalidRecordedValue(double recordedValue)
    {
        return new InvalidRecordedValue(recordedValue);
    }

    public static Error DuplicateRecordedDate(DateOnly recordedDate)
    {
        return new DuplicateRecordedDate(recordedDate);
    }
}
