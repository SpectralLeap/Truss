using Truss.Application.Abstractions.Domain;

namespace Truss.Domain.Tests.Entities.TestDomain;

internal sealed record NotWordAggregateId(Guid value) : AggregateRootId<Guid>(value);