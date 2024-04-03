namespace Truss.Application.Tests.Unit.Queries.TestApplication;

public sealed class ThingStore
{
    public int GetThing(int thingToGet)
    {
        return thingToGet + 1;
    }
}