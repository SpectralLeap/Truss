using Truss.Drivers;
using Truss.Dsl.Arguments;

namespace Truss.Tests;

public sealed class ApplyNameDriver(IIntegrationBus integrationBus) : Driver<ApplyNameAction>(integrationBus)
{
    public override void Drive(DslArgs args)
    {
    }
}