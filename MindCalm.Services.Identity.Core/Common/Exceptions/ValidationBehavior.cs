using System.Reflection;
using FluentValidation;
using MediatR;
using MindCalm.Services.Identity.Core.Common.Models;

namespace MindCalm.Services.Identity.Core.Common.Exceptions;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // 1. If there are no validators for this request, just continue to the handler
        if (!validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);

        // 2. Run all validators asynchronously
        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // 3. Extract all failure messages
        var failures = validationResults
            .Where(r => r.Errors.Count > 0)
            .SelectMany(r => r.Errors)
            .Select(f => f.ErrorMessage)
            .ToList();

        // 4. If there are errors, short-circuit the pipeline and return Result.Failed
        if (failures.Count != 0)
        {
            // Reflection Wizardry: We need to create a Result<T>.Failed() but we only know TResponse at runtime
            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultInnerType = typeof(TResponse).GetGenericArguments()[0]; // Gets the 'AuthResult' or 'RegisterResult'
                
                var failedMethod = typeof(Result<>)
                    .MakeGenericType(resultInnerType)
                    .GetMethod("Failed", BindingFlags.Public | BindingFlags.Static);

                if (failedMethod != null)
                {
                    // Parameters for Result.Failed(T? data = null, string? message = null, Enumerable<string>? errors = null)
                    var response = failedMethod.Invoke(null, new object?[] { null, "Validation Failed", failures });
                    return (TResponse)response!;
                }
            }

            // Fallback just in case a handler doesn't return Result<T>
            throw new ValidationException("Validation failed", validationResults.SelectMany(r => r.Errors));
        }

        // 5. If everything is valid, proceed to the actual CommandHandler
        return await next(cancellationToken);
    }
}