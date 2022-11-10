using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace Nexus.Portal.Infrastructure.Polly;

public static class HttpClientBuilderExtensions
{
    public static IHttpClientBuilder AddDefaultRetryPolicy(this IHttpClientBuilder builder)
    {
        var delay = Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5);
        var policy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(delay, (outcome, timespan, retryAttempt, context) =>
            {
                if (outcome.Exception == null)
                {
                    if (context.TryGetLogger(out var logger))
                    {
                        logger.LogWarning(
                            "HTTP {RequestMethod} {RequestPath} responded {StatusCode}. Retrying attempt {RetryAttempt} in {RetryDelay}.",
                            outcome.Result.RequestMessage?.Method.Method,
                            outcome.Result.RequestMessage?.RequestUri?.ToString(), outcome.Result.StatusCode,
                            retryAttempt,
                            timespan);
                    }
                }
            });
        return builder.AddPolicyHandler(policy);
    }
}
