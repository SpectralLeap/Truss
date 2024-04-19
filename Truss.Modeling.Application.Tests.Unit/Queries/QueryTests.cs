using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Modeling.Application.Tests.Unit.Queries.TestApplication;
using Truss.Modeling.Infrastructure;

namespace Truss.Modeling.Application.Tests.Unit.Queries;

public sealed class QueryTests
{
    private readonly IServiceProvider _serviceProvider;
    private const int GoodResult = 1;

    public QueryTests()
    {
        _serviceProvider = new ServiceCollection()
#if NET461 || NET47 || NET48
                .AddMediatR([typeof(ThingStore).Assembly])
#else
                .AddMediatR(c => 
                    c.RegisterServicesFromAssemblies([typeof(ThingStore).Assembly]))
#endif
                .AddTruss(c => 
                    c.AddModule<TestModule>()
                )
                .AddSingleton<ThingStore>()
                .BuildServiceProvider()
            ;
    }

    private async Task<int> RunGoodQuery()
    {
        var result = await _serviceProvider.GetService<IQueryBus>()!.SendQuery<ThingQuery, ThingQueryResult>(new ThingQuery(0));
        return result.SuccessValue.ThingGotten;
    }

    [Fact]
    public async Task gets_the_thing()
    {
        var result = await RunGoodQuery();
        Assert.Equal(GoodResult, result);
    }
}