using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Truss.Monads.Results;

namespace Truss.Infrastructure.FluentValidation;

public sealed class FluentValidationInstallationStep : IInstallationStep
{
    public void Run(
        IServiceCollection services,
        IConfiguration configuration,
        ModuleManifest moduleManifest
    )
    {
        foreach (var assembly in moduleManifest.Assemblies)
        {
            services.AddValidatorsFromAssembly(assembly);
        }
    }
}

public sealed class FluentValidationBehavior<TRequest, TResponse>
    : ResultPipelineBehavior<TRequest, TResponse>
    where TResponse : IResult
    where TRequest : notnull
{
    private readonly ILogger<FluentValidationBehavior<TRequest, TResponse>> _logger;
    private readonly IServiceProvider _provider;

    public FluentValidationBehavior(
        ILogger<FluentValidationBehavior<TRequest, TResponse>> logger,
        IServiceProvider provider
    )
    {
        _logger = logger;
        _provider = provider;
    }

    public override async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        _logger.LogDebug(
            "Validating request {RequestName}",
            typeof(TRequest).Name
        );

        var validator = _provider
            .GetService<IValidator<TRequest>>();

        if (validator is null)
        {
            _logger.LogWarning(
                "No validator found for request {RequestName}",
                typeof(TRequest).Name
            );

            return await next();
        }

        _logger.LogDebug(
            "Using validator {ValidatorName}",
            validator.GetType().Name
        );

        var result = await validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);

        if (result.IsValid) return await next();

        return Fail(
            FailureType.Validation,
            [
                "Request validation failed",
                ..result.Errors
                    .Select(error => error.ErrorMessage)
            ]);
    }
}