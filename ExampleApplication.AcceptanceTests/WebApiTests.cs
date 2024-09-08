using Truss.Testing;

namespace ExampleApplication.AcceptanceTests;

public sealed class WebApiTests
{
    private readonly DomainSpecificLanguageFactory _domainSpecificLanguage = new();
    
    [Fact]
    public async Task UserCanLogin()
    {
        var dsl = _domainSpecificLanguage.GetDomainSpecificLanguage<ExampleServiceDomainSpecificLanguage>();
        
        await dsl.AssertHeartbeat();
    }
}