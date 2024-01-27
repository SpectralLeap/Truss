// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Truss.Results;
using Truss.Results.Contextual;


Console.WriteLine("Hello, World!");

var summary = BenchmarkRunner.Run<Benchmarks>();


internal sealed record Number(int Value);

public sealed record String(string Value);


internal sealed class NumberProvider
{
    private readonly int _value = Random.Shared.Next();
    private readonly int _delay = Random.Shared.Next(1, 100);
    
    public Number GetNumberSync(int i = 0)
    {
        return new Number(_value + i);
    }
        

    public async Task<Number> GetNumberAsync(int i = 0)
    {
        await Task.Delay(_delay).ConfigureAwait(false);
        
        return new Number(_value + i);
    }

    public async Task<Result<Number>> GetNumberResultAsync()
    {
        return await GetNumberAsync().ConfigureAwait(false);
    }
}

internal sealed class StringProvider
{
    private readonly string _value = Guid.NewGuid().ToString();
    private readonly int _delay = Random.Shared.Next(10, 100);

    public String GetStringSync()
    {
        return new String(_value);
    }

    public Result<String> GetStringResult()
    {
        return Result.Success(GetStringSync());
    }
    
    public async Task<String> GetStringAsync(String? s = null)
    {
        await Task.Delay(_delay).ConfigureAwait(false);

        return new String(s?.Value ?? _value);
    }
    
    public async Task<Result<String>> GetStringResultAsync()
    {
        return await GetStringAsync().ConfigureAwait(false);
    }
}


public class Benchmarks
{
    private readonly IServiceProvider _provider = new ServiceCollection()
            .AddTransient<ResolutionContextFactory>()
            .AddTransient<NumberProvider>()
            .AddTransient<StringProvider>()
            .AddLogging()
            .BuildServiceProvider()
        ;

    private StringProvider strings => _provider.GetService<StringProvider>()!;
    private NumberProvider numbers => _provider.GetService<NumberProvider>()!;

    [Benchmark]
    public async Task<Result<String>> result () => await new ResolutionContextFactory()
        .From(await strings.GetStringResultAsync().ConfigureAwait(false))
        .DoAsync(_ => numbers.GetNumberAsync())
        .DoAsync(_ => strings.GetStringSync())
        .ThenAsync(async s => await strings.GetStringAsync(s).ConfigureAwait(false))
        .ThenAsync(async _ => await numbers.GetNumberResultAsync().ConfigureAwait(false))
        .ThenAsync(_ => strings.GetStringSync())
        .ThenAsync(s => s)
        .DoAsync(s => Console.WriteLine(s.Value))
        .Resolve()
        .ConfigureAwait(false)
    ;

    [Benchmark]
    public async Task<Result<String>> resultByRef () => await new ResolutionContextFactory()
        .FromRef(await strings.GetStringResultAsync().ConfigureAwait(false))
        .DoAsync(_ => numbers.GetNumberAsync())
        .DoAsync(_ => strings.GetStringSync())
        .ThenAsync(async s => await strings.GetStringAsync(s).ConfigureAwait(false))
        .ThenAsync(async _ => await numbers.GetNumberResultAsync().ConfigureAwait(false))
        .ThenAsync(_ => strings.GetStringSync())
        .ThenAsync(s => s)
        .DoAsync(s => Console.WriteLine(s.Value))
        .Resolve()
        .ConfigureAwait(false)
    ;

}