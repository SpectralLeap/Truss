using MediatR;
using Truss.Monads.Results;

namespace ExampleApplication.WebApi.Services;

public sealed class GreeterPipelineBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, Result<GreetResult>>
    where TResponse : IResult<GreetResult> where TRequest : notnull
{
    public async Task<Result<GreetResult>> Handle(TRequest request, RequestHandlerDelegate<Result<GreetResult>> next, CancellationToken cancellationToken)
    {
        var x = await next();

        if (x.Failed) return x;

        var greeting = x.SuccessValue;

        if (greeting.Greeting.Contains("a"))
        {
            return Result.Success(new GreetResult
            {
                Greeting = "The greeting had an a",
            });
        }

        return x;
    }
}