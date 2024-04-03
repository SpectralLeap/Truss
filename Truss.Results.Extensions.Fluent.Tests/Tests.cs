using System;
using System.Threading.Tasks;
using Xunit;

namespace Truss.Results.Extensions.Fluent.Tests;

public sealed class Tests
{
    [Fact]
    public async Task DoesThing()
    {
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


    private Result<int> Return5()
    {
        return 5.AsResult();
    }
}