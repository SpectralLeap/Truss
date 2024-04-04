using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Application.Cqrs.Queries;
using Truss.Modeling.Application.Tests.Unit.Queries.TestApplication;

namespace Truss.Modeling.Application.Tests.Unit.Queries;

public sealed class QueryTests
{
    private readonly IServiceProvider _serviceProvider;
    private const int GoodResult = 1;

    public QueryTests()
    {
        _serviceProvider = new ServiceCollection()
                .AddMediatR(c =>
                    c.RegisterServicesFromAssembly(typeof(ThingQuery).Assembly))
                .AddCqrs()
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