using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Nexus.Todo.Api.Infrastructure.Extensions;

namespace Nexus.Todo.Api.Infrastructure.Behaviours;

public class ValidatorBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
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
        var typeName = TypeNameHelper.GetTypeDisplayName(request);

        _logger.LogInformation("---- Validating command {CommandType}", typeName);

        var failures = new List<ValidationFailure>();
        foreach (var validator in _validators.Where(v => v.CanValidateInstancesOfType(typeof(TRequest))))
        {
            var context = new ValidationContext<TRequest>(request);
            var result = await validator.ValidateAsync(context, cancellationToken);
            failures.AddRange(result.Errors);
        }

        if (failures.Any())
        {
            _logger.LogWarning(
                "Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName,
                request, failures);

            throw new ValidationException($"{failures.Count} errors found while validating the request.", failures);
        }

        return await next();
    }
}
