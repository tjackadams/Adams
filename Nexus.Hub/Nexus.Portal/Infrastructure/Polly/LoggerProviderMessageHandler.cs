using Polly;

namespace Nexus.Portal.Infrastructure.Polly;

public class LoggerProviderMessageHandler<T> : DelegatingHandler
{
    private readonly ILogger<T> _logger;
    public LoggerProviderMessageHandler(ILogger<T> logger) => _logger = logger;
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var context = new Context();
        context.TryAdd(PolicyContextItems.Logger, _logger);
        request.SetPolicyExecutionContext(context);

        return base.SendAsync(request, cancellationToken);
    }
}
