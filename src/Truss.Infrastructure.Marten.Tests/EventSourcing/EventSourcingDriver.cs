using Marten;
using Microsoft.Extensions.DependencyInjection;
using Truss.Infrastructure.Marten.Tests.EventSourcing.TestApplication;
using Truss.Infrastructure.Tests.Dependencies;
using Truss.Modeling.Application.Cqrs.EventSourcing.Persistence;
using Truss.Testing;
using Truss.Testing.Services;

namespace Truss.Infrastructure.Marten.Tests.EventSourcing;

public sealed class EventSourcingDriver(
    IAggregateRepository aggregateRepository
)
    : Driver
{
    [BaseServices]
    private static readonly IServiceCollection Services = new ServiceCollection()
            .AddMarten(o =>
            {
                o.Connection(
                    PostgresDatabaseSharedDependency.ConnectionString!
                );

                o.CreateDatabasesForTenants(
                    c => c.MaintenanceDatabase(
                        PostgresDatabaseSharedDependency.ConnectionString!
                    ).ForTenant()
                    .CheckAgainstPgDatabase()
                );
            })
            .ApplyAllDatabaseChangesOnStartup()
            .Services
            .AddTruss(c => c.AddMartenServices())
        ;


    public async Task AssertWrites()
    {
        var counter = Counter.New();
        counter.IncrementNumber();
        counter.IncrementNumber();

        var result = await aggregateRepository.StoreAsync(counter);

        Assert.True(result.Succeeded);
    }

    public async Task AssertReads()
    {
        var counter = Counter.New();
        counter.IncrementNumber();
        counter.IncrementNumber();

        await aggregateRepository.StoreAsync(counter);

        var newCounterResult = await aggregateRepository.LoadAsync<Counter>(counter.Id);

        var newCounter = newCounterResult.SuccessValue;

        Assert.Equal(counter.Id, newCounter.Id);
        Assert.Equal(counter.Number, newCounter.Number);
    }

    public async Task AssertPendingEventsAreNotStored()
    {
        var counter = Counter.New();
        counter.IncrementNumber();
        counter.IncrementNumber();

        await aggregateRepository.StoreAsync(counter);

        var newCounterResult = await aggregateRepository.LoadAsync<Counter>(counter.Id);

        var newCounter = newCounterResult.SuccessValue;

        Assert.Empty(newCounter.PendingEvents);
    }

    public async Task AssertPendingEventsAreClearedAfterWrite()
    {
        var counter = Counter.New();
        counter.IncrementNumber();
        counter.IncrementNumber();

        await aggregateRepository.StoreAsync(counter);

        Assert.Empty(counter.PendingEvents);
    }
}