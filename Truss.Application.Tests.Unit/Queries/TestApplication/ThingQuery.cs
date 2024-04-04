using Truss.Application.Cqrs.Queries;

namespace Truss.Application.Tests.Unit.Queries.TestApplication;

public sealed record ThingQuery(int ThingToGet) : Query<ThingQueryResult>;