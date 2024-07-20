using Truss.Testing.Tests.Drivers;

namespace Truss.Testing.Tests.Services;

public class FixtureRequestingUnregisteredDependency(RegistrationStore registrationStore) : Fixture
{
    
}