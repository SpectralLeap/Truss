using Truss.Application.Abstractions.Queries;

namespace Truss.Application.Tests.Queries.TestApplication;

public sealed record ThingQuery(int ThingToGet) : Query<ThingQueryResult>;