using System.Diagnostics.CodeAnalysis;
using Polly;

namespace Nexus.Portal.Infrastructure.Polly;

public static class PollyContextExtensions
{
    public static bool TryGetLogger(this Context context, [NotNullWhen(true)] out ILogger? logger)
    {
        if (context.TryGetValue(PolicyContextItems.Logger, out var loggerObject) && loggerObject is ILogger theLogger)
        {
            logger = theLogger;
            return true;
        }

        logger = null;
        return false;
    }
}
