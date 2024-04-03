using Truss.Application.Abstractions.Domain;

namespace Truss.Domain.Tests.Unit.EventSourcing.TestDomain;

public sealed record TreeId(Guid value) : AggregateRootId<Guid>(value);