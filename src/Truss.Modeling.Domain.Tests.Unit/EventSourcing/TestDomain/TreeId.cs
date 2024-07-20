using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Domain.Tests.Unit.EventSourcing.TestDomain;

public sealed record TreeId(Guid Value) : AggregateRootId<Guid>(Value);