using Truss.Testing.Tests.Drivers;

namespace Truss.Testing.Tests.Services;

public class DriverRequestingUnregisteredDependency(RegistrationStore registrationStore)
    : Driver;