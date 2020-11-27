using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adams.Core.Extensions;
using Adams.Services.Smoking.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Adams.Services.Smoking.Api.Infrastructure.Behaviours
{
    public class ValidatorBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<ValidatorBehaviour<TRequest, TResponse>> _logger;
        private readonly IValidator<TRequest>[] _validators;

        public ValidatorBehaviour(IValidator<TRequest>[] validators,
            ILogger<ValidatorBehaviour<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var typeName = request.GetGenericTypeName();

            _logger.LogInformation("----- Validating command {CommandType}", typeName);

            var failures = new List<ValidationFailure>();

            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(request, cancellationToken);
                if (result.Errors.Any())
                {
                    failures.AddRange(result.Errors);
                }
            }

            if (failures.Any())
            {
                _logger.LogWarning(
                    "Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName,
                    request, failures);

                throw new SmokingDomainException(
                    $"Command Validation Errors for type {typeof(TRequest).Name}",
                    new ValidationException("Validation exception", failures));
            }

            return await next();
        }
    }
}