using Truss.Domain.Entities;

namespace Truss.Domain.Tests.Unit.Entities.TestDomain;

internal sealed record NotWordAggregateId(Guid value) : AggregateRootId<Guid>(value);