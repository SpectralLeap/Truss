using Xunit;

namespace Truss.Monads.Results.Extensions.Fluent.Tests;

public sealed class HandWrittenTests
{
    [Fact]
    public async Task HasDecentSyntax()
    {
        var y = Result.Success(2);
        
        if (y.Succeeded) Console.WriteLine(y.SuccessValue);
        
        if (y.Failed) Console.WriteLine(y.FailureMessage);

        y.Resolve(
            onSuccess: i => Console.WriteLine(i),
            onFailure: f => Console.WriteLine(f.GetMessage()));

        var x = await 3.AsResult()
                .Then(i => i + 1)
                .Then(x => Task.FromResult(x + 1))
                .Then(x => Task.FromResult(x + 1))
                .Then(x => x + 1)
                .Then(x => Result.Success(2))
                .Then(x => Task.FromResult(Result.Success(2))) 
                .And(x => x + 1)
                .Then((i, ii) => i + 1)
                .And(x => x + 1)
                .Do((a, x) =>
                {
                    var y =x + 1;
                })
                .Do((a, x) => Result.Success())
                .Do((a, x) => Result.Success(2))
                .Do((a, x) => Result.Fail())
                .Do((a, x) => Task.FromResult(Result.Success(2))) 
                .Do((a, b) =>
                {
                    // ReSharper disable once Xunit.XunitTestWithConsoleOutput
                    Console.WriteLine("");
                }) 
                .Then((a, b) => Task.FromResult(Result.Success(2)))
                .And(x => x + 1)
                .And(async (x, _) =>
                {
                    await Task.Delay(100);
                    return x + 1;
                })
                .And((x, _, _) => x + 1)
                .And((_, _, x, _) => x + 1)
                .And((x, y, z, a, b) => x + 1)
                .And((x, _, a, z, o, p) => x + 1)
                .Then((_, _, _, _ ,_, _, a) => Task.FromResult(Result.Success(a)))
            ;
        
    }

    [Fact]
    public void FailsOnFailure()
    {
        using var result = 9.AsResult()
                .Then(n => n + 1)
                .Then(n =>
                {
                    if (n > 9) return Result.Fail("no");

                    return Result.Success();
                })
                .Then(_ => "Done")
            ;
        
        Assert.True(result.Failed);
    }

    [Fact]
    public void DisposesTheThings()
    {
        var result = GetThingAsResult()
            ;
        
        Assert.False(result.SuccessValue.IsDisposed);
    }

    [Fact]
    public void DisposesTheThingIfFromAChain()
    {
        var result = GetThingFromResolutionChain()
            ;
            
        Assert.True(result.SuccessValue.IsDisposed);
    }

    private Result<DisposableThing> GetThingAsResult()
    {
        return Result.Success(new DisposableThing());
    }

    private Result<DisposableThing> GetThingFromResolutionChain()
    {
        using var x = 9.AsResult()
                .Then(_ => new DisposableThing())
            ;

        return x;
    }
}