namespace Truss.Monads.Results;

public interface IResult
{
    public bool Succeeded { get; }
    public bool Failed { get; }
    public object SuccessObject { get; }
    public FailureDetails FailureDetails { get; }
    public string FailureMessage { get; }
}