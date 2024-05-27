namespace Truss.Monads.Results;

public interface IResult
{
    public bool Succeeded { get; }
    public object SuccessObject { get; }
    public bool Failed { get; }
    public FailureDetails? FailureDetails { get; }
}