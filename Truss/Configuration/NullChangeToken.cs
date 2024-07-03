using Microsoft.Extensions.Primitives;

namespace Truss.Configuration;

public sealed class NullChangeToken : IChangeToken
{
    public static readonly NullChangeToken Instance = new();

    public bool HasChanged => false;
    public bool ActiveChangeCallbacks => false;
    
    public IDisposable RegisterChangeCallback(Action<object> callback, object state)
    {
        throw new InvalidOperationException("The configuration is empty");
    }

    private NullChangeToken()
    {
        
    }

}