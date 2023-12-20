using Truss.Dsl.Arguments;

namespace Truss.Drivers;

public abstract class Driver<TAction>(IIntegrationBus integrationBus)
{
    public abstract void Drive(DslArgs args);

    protected void Report<TReport>(TReport report)
    {
    }
}