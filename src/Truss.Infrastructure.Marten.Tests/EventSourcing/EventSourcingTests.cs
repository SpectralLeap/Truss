namespace Truss.Infrastructure.Marten.Tests.EventSourcing;

[Collection(nameof(DriverFactoryLifetimeAdapter))]
public sealed class EventSourcingTests
{

    private readonly DriverFactoryLifetimeAdapter _driverFactoryLifetimeAdapter;

    [Fact]
    public async Task writes_to_the_event_stream()
    {
        var driver = await _driverFactoryLifetimeAdapter.DriverFactory
            .GetDriverAsync<EventSourcingDriver>();

        await driver.AssertWrites();
    }

    [Fact]
    public async Task reads_from_the_event_stream()
    {
        var driver = await _driverFactoryLifetimeAdapter.DriverFactory.GetDriverAsync<EventSourcingDriver>();

        await driver.AssertReads();
    }

    [Fact]
    public async Task pending_events_are_cleared_after_write()
    {
         var driver = await _driverFactoryLifetimeAdapter.DriverFactory.GetDriverAsync<EventSourcingDriver>();

         await driver.AssertPendingEventsAreClearedAfterWrite();
    }

    [Fact]
    public async Task pending_events_are_not_stored()
    {
        var driver = await _driverFactoryLifetimeAdapter.DriverFactory.GetDriverAsync<EventSourcingDriver>();

        await driver.AssertPendingEventsAreNotStored();
    }

    public EventSourcingTests(
        DriverFactoryLifetimeAdapter driverFactoryLifetimeAdapter
    )
    {
        _driverFactoryLifetimeAdapter = driverFactoryLifetimeAdapter;
    }
}