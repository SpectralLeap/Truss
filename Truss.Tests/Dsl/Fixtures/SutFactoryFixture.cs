using Microsoft.Extensions.DependencyInjection;
using Truss.Dsl;

namespace Truss.Tests.Dsl.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class SutFactoryFixture : DslFactory
{
    public SutFactoryFixture() : base(new ServiceCollection()
        .AddSingleton<IUserInfo, UserInfo>()
        .AddSingleton<RandomGuid>()
    )
    {
        RegisterOverrideSet("admin", new ServiceCollection().AddSingleton<IUserInfo, AdminInfo>());
    }
}