using Microsoft.Extensions.Logging;

namespace Truss.Results.Contextual;

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
        if (result is null) throw new ArgumentNullException(nameof(result));

        return new ResolutionStep<T>(_logger, result);
    }

    public ResolutionStepRef<T> FromRef<T>(Result<T> result)
    {
         if (result is null) throw new ArgumentNullException(nameof(result));
 
         return new ResolutionStepRef<T>(_logger, result);       
    }
}