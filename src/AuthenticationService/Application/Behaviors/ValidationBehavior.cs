using FluentValidation;
using MediatR;

using Shared.Results;

namespace Application.Behaviors;
public class ValidationBehavior<TRequest, TResponse> (IEnumerable<IValidator> validators): 
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var failures = validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .Select(f => Error.ValidationFailed(f.ErrorMessage))
            .ToList();

        return failures.Count == 0 ? await next() : (dynamic)failures;
    }
}
