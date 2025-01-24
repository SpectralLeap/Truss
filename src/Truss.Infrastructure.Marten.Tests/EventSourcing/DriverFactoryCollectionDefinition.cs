namespace Truss.Infrastructure.Marten.Tests.EventSourcing;

[CollectionDefinition(nameof(DriverFactoryLifetimeAdapter))]
public sealed class DriverFactoryCollectionDefinition
    : ICollectionFixture<DriverFactoryLifetimeAdapter>;