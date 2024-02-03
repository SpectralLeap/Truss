namespace Truss.Results.Extensions.Functional;

public sealed class ResolutionContext
{
    public ResolutionFactory<T> Using<T>(Result<T> result)
    {
        return new ResolutionFactory<T>(result);
    }
}

public sealed class ResolutionFactory<T>
{
    private readonly Result<T> _result;

    public ResolutionFactory(Result<T> result)
    {
        _result = result;
    }
}