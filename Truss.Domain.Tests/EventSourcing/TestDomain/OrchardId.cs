using Truss.Application.Abstractions.Domain;

namespace Truss.Domain.Tests.EventSourcing.TestDomain;

public sealed record OrchardId : AggregateRootId<Guid>
{
    public OrchardId(Guid value) : base(value)
    {
    }
}