using Truss.Testing.Dsl.Tests.Drivers;

namespace Truss.Testing.Dsl.Tests.Services;

public class DslRequestingUnregisteredDependency(RegistrationStore registrationStore) : Dsl
{
    
}