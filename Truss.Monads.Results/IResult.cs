namespace Truss.Monads.Results;

#pragma warning disable CS0108, CS0114
public interface IResult
{
    public bool Succeeded { get; }
    public object SuccessObject { get; }
    public bool Failed { get; }
    public FailureDetails? FailureDetails { get; }
}