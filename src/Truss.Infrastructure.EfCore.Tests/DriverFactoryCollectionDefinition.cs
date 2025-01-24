namespace Truss.Infrastructure.EfCore.Tests;

[CollectionDefinition(nameof(DriverFactoryLifetimeAdapter))]
public sealed class DriverFactoryCollectionDefinition
    : ICollectionFixture<DriverFactoryLifetimeAdapter>;