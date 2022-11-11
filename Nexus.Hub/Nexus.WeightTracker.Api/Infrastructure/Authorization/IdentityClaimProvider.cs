using Microsoft.Identity.Web;
using Nexus.WeightTracker.Api.Infrastructure.ErrorHandling;

namespace Nexus.WeightTracker.Api.Infrastructure.Authorization;

public class IdentityClaimProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public IdentityClaimProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetObjectId()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new ProblemDetailsException(StatusCodes.Status500InternalServerError);
        }

        var objectId = _httpContextAccessor.HttpContext.User.GetObjectId();
        if (objectId is null)
        {
            throw new ProblemDetailsException(StatusCodes.Status500InternalServerError);
        }

        return objectId;
    }
}
