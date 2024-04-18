using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.Services;

namespace Truss.Testing.Tests.Fixtures;

public class RegisteredFixture(IServiceProvider provider) : Fixture
{
    
    [BaseServices]
    private static readonly IServiceCollection BaseServices = new ServiceCollection()
            .AddSingleton<IUserInfo, UserInfo>()
            .AddSingleton<RandomGuid>()
        ;
        
    [ServiceOverride("admin")]
    private static readonly IServiceCollection AdminServices = new ServiceCollection()
        .AddSingleton<IUserInfo, AdminInfo>();
    
    public Guid ProviderGuid => provider.GetService<RandomGuid>()!.Guid;

    public Guid TypeGuid => Guid.NewGuid();
    
    public bool IsAdmin => provider.GetService<IUserInfo>()!.IsAdmin;
}