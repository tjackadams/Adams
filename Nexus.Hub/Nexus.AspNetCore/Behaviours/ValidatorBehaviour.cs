using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Nexus.AspNetCore.Helpers;
using Microsoft.AspNetCore.Http;

namespace Nexus.AspNetCore.Behaviours;
public class ValidatorBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    private readonly ILogger<ValidatorBehaviour<TRequest, TResponse>> _logger;
    private readonly IValidator[] _validators;

    public ValidatorBehaviour(ILogger<ValidatorBehaviour<TRequest, TResponse>> logger, IEnumerable<IValidator> validators)
    {
        _logger = logger;
        _validators = validators.ToArray();
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var typeName = TypeNameHelper.GetTypeDisplayName(request);

            _logger.LogInformation("---- Validating command {CommandType}", typeName);

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators
                    .Where(v => v.CanValidateInstancesOfType(typeof(TRequest)))
                    .Select(v => v.ValidateAsync(context, cancellationToken))
                    );

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToArray();

            if (failures.Any())
            {
                _logger.LogWarning(
                    "Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName,
                    request, failures);

                var errors = failures
                    .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                    .ToDictionary(
                        e => e.Key,
                        e => e.ToArray());

                return (TResponse)((IResult)TypedResults.ValidationProblem(errors));
            }
        }


        return await next();
    }
}
