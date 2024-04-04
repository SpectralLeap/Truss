using Truss.Domain.Entities;

namespace Truss.Domain.Tests.Unit.EventSourcing.TestDomain;

public sealed record TreeId(Guid value) : AggregateRootId<Guid>(value);