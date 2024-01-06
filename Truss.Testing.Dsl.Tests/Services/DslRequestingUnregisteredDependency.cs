using Truss.Testing.Dsl.Tests.Core;

namespace Truss.Testing.Dsl.Tests.Services;

public class DslRequestingUnregisteredDependency(RegistrationStore registrationStore) : Dsl
{
    
}