﻿using FluentResults;

namespace Nexus.WeightTracker.Api.Domain.ErrorHandling;

public static class DomainErrorProvider
{
    public static Error InvalidRecordedValue(decimal recordedValue)
    {
        return new InvalidRecordedValue(recordedValue);
    }
}