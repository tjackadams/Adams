using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Nexus.WeightTracker.Api.Infrastructure.ErrorHandling;

public static class WebApplicationErrorHandlingExtensions
{
    public static WebApplication UseErrorHandling(this WebApplication app)
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                var exceptionHandler = context.Features.Get<IExceptionHandlerPathFeature>();

                if (exceptionHandler?.Error is ProblemDetailsException problemDetailsException)
                {
                    context.Response.StatusCode = problemDetailsException.Details.Status.GetValueOrDefault();

                    await context.Response.WriteAsJsonAsync(problemDetailsException.Details);

                    return;
                }

                if (exceptionHandler?.Error is ValidationException validationException)
                {
                    var errors = validationException.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            e => e.Key,
                            e => e.Select(x => x.ErrorMessage).ToArray());

                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    await context.Response.WriteAsJsonAsync(new HttpValidationProblemDetails(errors)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please refer to the errors property for additional details."
                    });

                    return;
                }

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError
                });
            });
        });

        return app;
    }
}
