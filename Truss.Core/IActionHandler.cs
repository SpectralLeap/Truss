namespace Truss.Core;

public interface IActionHandler<in TAction, out TResult>
{
    public TResult Handle(params string[] args);
}

public interface IResultAsserter<in TResult>
{
    
}