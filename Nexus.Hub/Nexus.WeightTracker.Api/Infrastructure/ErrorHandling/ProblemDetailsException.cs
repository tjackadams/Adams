using System.Text;
using Microsoft.AspNetCore.Mvc;
using Nexus.WeightTracker.Api.Infrastructure.ErroHandling;

namespace Nexus.WeightTracker.Api.Infrastructure.ErrorHandling;

public class ProblemDetailsException : Exception
{
    public ProblemDetailsException(int statusCode)
        : this(StatusCodeProblemDetails.Create(statusCode))
    {
    }

    public ProblemDetailsException(int statusCode, Exception innerException)
        : this(StatusCodeProblemDetails.Create(statusCode), innerException)
    {
    }

    public ProblemDetailsException(int statusCode, string title)
        : this(StatusCodeProblemDetails.Create(statusCode, title), null)
    {
    }

    public ProblemDetailsException(int statusCode, string title, Exception innerException)
        : this(StatusCodeProblemDetails.Create(statusCode, title), innerException)
    {
    }

    public ProblemDetailsException(ProblemDetails details)
        : this(details, null)
    {
        Details = details;
    }

    public ProblemDetailsException(ProblemDetails details, Exception? innerException)
        : base($"{details.Type} : {details.Title}", innerException)
    {
        Details = details;
    }

    public ProblemDetails Details { get; }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine($"Type    : {Details.Type}");
        stringBuilder.AppendLine($"Title   : {Details.Title}");
        stringBuilder.AppendLine($"Status  : {Details.Status}");
        stringBuilder.AppendLine($"Detail  : {Details.Detail}");
        stringBuilder.AppendLine($"Instance: {Details.Instance}");

        return stringBuilder.ToString();
    }
}
