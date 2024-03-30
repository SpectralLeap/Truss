using Microsoft.Extensions.Logging;

namespace Truss.Results.Extensions.Contextual;

public sealed class ResolutionContextFactory
{
    private readonly ILogger<ResolutionContextFactory>? _logger;

    public ResolutionContextFactory(ILogger<ResolutionContextFactory>? logger = null)
    {
        _logger = logger;
    }
    
    public ResolutionStep<T> From<T>(T result)
    {
        return From(Result.Success(result));
    }
    
    public ResolutionStep<T> From<T>(Result<T> result)
    {
        return new ResolutionStep<T>(_logger, result);
    }

    public ValueResolutionStep<T> FromValue<T>(Result<T> result)
    {
         return new ValueResolutionStep<T>(_logger, result);       
    }
}