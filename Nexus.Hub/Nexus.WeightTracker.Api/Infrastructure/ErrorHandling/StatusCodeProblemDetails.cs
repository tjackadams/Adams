using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Nexus.WeightTracker.Api.Infrastructure.ErroHandling;

public class StatusCodeProblemDetails : ProblemDetails
{
    public StatusCodeProblemDetails(int statusCode)
    {
        SetDetails(this, statusCode);
    }

    public static ProblemDetails Create(int statusCode)
    {
        var details = new ProblemDetails();

        SetDetails(details, statusCode);

        return details;
    }

    internal static ProblemDetails Create(int statusCode, string title)
    {
        var details = Create(statusCode);

        details.Title = title;

        return details;
    }

    private static void SetDetails(ProblemDetails details, int statusCode)
    {
        details.Status = statusCode;
        details.Type = GetDefaultType(statusCode);
        details.Title = ReasonPhrases.GetReasonPhrase(statusCode);
    }

    internal static string GetDefaultType(int statusCode)
    {
        return $"https://httpstatuses.io/{statusCode}";
    }
}
