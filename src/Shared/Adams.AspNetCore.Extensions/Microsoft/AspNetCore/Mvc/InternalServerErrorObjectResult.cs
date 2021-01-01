using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adams.AspNetCore.Extensions.Microsoft.AspNetCore.Mvc
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
