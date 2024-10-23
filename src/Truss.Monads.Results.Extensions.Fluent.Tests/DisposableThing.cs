namespace Truss.Monads.Results.Extensions.Fluent.Tests;

public sealed class DisposableThing : IDisposable
{
    public bool IsDisposed;
    
    public void Dispose()
    {
        IsDisposed = true;
    }
}