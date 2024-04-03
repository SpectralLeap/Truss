using Truss.Application.Abstractions.Domain;

namespace Truss.Domain.Tests.EventSourcing.TestDomain;

public sealed record TreeId(Guid value) : AggregateRootId<Guid>(value);