namespace Truss.Monads.Results;

public sealed class ResolutionException : Exception
{
    public ResolutionException(IReadOnlyCollection<string> msgs) : this(msgs.ToArray())
    {
        
    }
    public ResolutionException(params string[] msg) : base(string.Join("\n", msg))
    {
        
    }
}