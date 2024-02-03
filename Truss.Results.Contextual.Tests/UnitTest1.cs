using Microsoft.Extensions.DependencyInjection;
using Truss.Results.Extensions.Contextual;

namespace Truss.Results.Contextual.Tests;

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
                .Then(a => a)
                .DoAsync(_ => numbers.GetNumberAsync())
                .PerformAsync(_ => strings.GetStringSync())
                .ThenAsync(async s => await strings.GetStringAsync(s))
                .ThenAsync(async _ => await numbers.GetNumberResultAsync())
                .ThenAsync(_ => strings.GetStringSync())
                .ThenAsync(s => s)
                .PerformAsync(s => Console.WriteLine(s.Value))
                .Resolve()
            ;

        var x = strings.GetStringAsync();

        Assert.True(result.Succeeded);
        Assert.True(Guid.TryParse(result.SuccessValue.Value, out _));
    }
    
    [Fact]
    public void Test2()
    {
        var result = contextFactory.From("42")
                .Then(s => int.Parse(s))
                .Then(i => Result.Success(i / 2.0))
                .Then(b => Result.Success(3))
                .Perform(i => i * 2)
                .Perform(_ => (Result<int>)Result.Fail("bad"))
                .Perform(i => new List<int>().Add(i))
                .Perform(() => Console.WriteLine("Here"))
                .Then(_ => Result.Success())
                .Resolve()
            ;
            
        Assert.True(result.Failed);
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