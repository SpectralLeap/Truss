using Truss.Testing.Tests.Drivers;

namespace Truss.Testing.Tests.Services;

public class DomainSpecificLanguageRequestingUnregisteredDependency(RegistrationStore registrationStore)
    : DomainSpecificLanguage;