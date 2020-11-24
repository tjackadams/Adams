using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adams.Services.Smoking.Api.Infrastructure.ActionResults
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