using Truss.Modeling.Application.Cqrs.Queries;

namespace Truss.Modeling.Application.Tests.Unit.Queries.TestApplication;

public sealed record ThingQuery(int ThingToGet) : Query<ThingQueryResult>
{
    public int ThingToGet { get; } = ThingToGet;
}