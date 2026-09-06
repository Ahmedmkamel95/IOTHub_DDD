using CIOT.Common.Results;
using FluentValidation;
using MediatR;

namespace CIOT.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationFailures = (await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (validationFailures.Count != 0)
        {
            var firstError = validationFailures[0];
            var error = Error.Validation(
                firstError.PropertyName,
                firstError.ErrorMessage);

            if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.Failure(error);
            }

            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var valueType = typeof(TResponse).GenericTypeArguments[0];
                var failureMethod = typeof(Result)
                    .GetMethods()
                    .Single(method =>
                        method.Name == nameof(Result.Failure)
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 1
                        && method.GetParameters().Length == 1
                        && method.GetParameters()[0].ParameterType == typeof(Error))
                    .MakeGenericMethod(valueType);

                return (TResponse)failureMethod.Invoke(null, [error])!;
            }

            throw new ValidationException(validationFailures);
        }

        return await next();
    }
}
