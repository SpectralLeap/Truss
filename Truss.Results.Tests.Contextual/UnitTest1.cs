using Microsoft.Extensions.DependencyInjection;
using Truss.Results.Contextual;

namespace Truss.Results.Tests.Contextual;

public sealed class UnitTest1
{
    private readonly IServiceProvider _provider = new ServiceCollection()
            .AddSingleton<ResolutionContext>()
            .BuildServiceProvider()
        ;

    private ResolutionContext factory => _provider.GetService<ResolutionContext>()!;
    
    [Fact]
    public void Test1()
    {
        var result = factory.Start(Result.Success("42"))
                .Then(s => int.Parse(s))
                .Then(i => Result.Success(i / 2.0))
                .Then(b => Result.Success(3))
                .Do(i => i * 2)
                .Do(i => Result.Success(4 * i))
                .Do(i => new List<int>().Add(i))
                .Do(async i => await Task.FromResult("cheese"))
                .Do(() => Console.WriteLine("Here"))
                .Then(i => Result.Success(i))
                .ThenAsync(i => Task.FromResult(i))
                .Resolve()
            ;
        
        Assert.True(result.Succeeded);
    }
    
    [Fact]
    public void Test2()
    {
        var result = factory.Start(Result.Success("42"))
                .Then(s => int.Parse(s))
                .Then(i => Result.Success(i / 2.0))
                .Then(b => Result.Success(3))
                .Do(i => i * 2)
                .Do(i => (Result<int>)Result.Fail("bad"))
                .Do(i => new List<int>().Add(i))
                .Do(() => Console.WriteLine("Here"))
                .Then(_ => Result.Success())
                .Resolve()
            ;
            
        Assert.False(result.Succeeded);
    }
}