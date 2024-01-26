using Microsoft.Extensions.DependencyInjection;
using Truss.Results.Contextual;

namespace Truss.Results.Tests.Contextual;

internal sealed record Number(int Value);
internal sealed record String(string Value);


internal sealed class NumberProvider
{
    private readonly int _value = Random.Shared.Next();
    private readonly int _delay = Random.Shared.Next(1, 100);
    
    public async Task<Number> GetNumber(int i = 0)
    {
        await Task.Delay(_delay);
        
        return new Number(_value + i);
    }

    public async Task<Result<Number>> GetNumberResult()
    {
        return await GetNumber();
    }
}

internal sealed class StringProvider
{
    private readonly string _value = Guid.NewGuid().ToString();
    private readonly int _delay = Random.Shared.Next(10, 100);

    public String GetString()
    {
        return new String(_value);
    }

    public Result<String> GetStringResult()
    {
        return Result.Success(GetString());
    }
    
    public async Task<String> GetStringAsync(String? s = null)
    {
        await Task.Delay(_delay);

        return new String(s?.Value ?? _value);
    }
    
    public async Task<Result<String>> GetStringResultAsync()
    {
        return await GetStringAsync();
    }
}

public sealed class UnitTest1
{
    private readonly IServiceProvider _provider = new ServiceCollection()
            .AddTransient<ResolutionContextFactory>()
            .AddTransient<NumberProvider>()
            .AddTransient<StringProvider>()
            .AddLogging()
            .BuildServiceProvider()
        ;

    private ResolutionContextFactory contextFactory => _provider.GetService<ResolutionContextFactory>()!;
    
    [Fact]
    public async Task Test1()
    {
        var numbers = _provider.GetService<NumberProvider>()!;
        var strings = _provider.GetService<StringProvider>()!;
        
        var result = await contextFactory
                .From(await strings.GetStringResultAsync())
                .DoAsync(_ => numbers.GetNumber())
                .ThenAsync(async s => await strings.GetStringAsync(s))
                .ThenAsync(s => s)
            ;

        Assert.Equal("x", result.Resolve().SuccessValue.Value);
    }
    
    [Fact]
    public void Test2()
    {
        var result = contextFactory.From("42")
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

    [Fact]
    public void Test3()
    {
        string rVal = "";
        var result = contextFactory.From("23")
            .And(s => "a" + s)
            .DoWith(
                f1: s => rVal = s,
                f2: s => rVal = s)
            ;
        
        Assert.Equal("a23", rVal);
    }
}