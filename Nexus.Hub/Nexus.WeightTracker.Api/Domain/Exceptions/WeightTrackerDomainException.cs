namespace Nexus.WeightTracker.Api.Domain.Exceptions;

public class WeightTrackerDomainException : Exception
{
    public IReadOnlyCollection<string> Errors { get; }
    public WeightTrackerDomainException(List<string> errors)
    : base("Please refer to the errors property for additional details.")
    {
        Errors = errors.AsReadOnly();
    }
}
