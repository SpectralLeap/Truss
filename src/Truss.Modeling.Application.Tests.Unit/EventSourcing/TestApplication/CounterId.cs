using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Application.Tests.Unit.EventSourcing.TestApplication;

public sealed record CounterId : AggregateRootId<Guid>
{
    public CounterId(Guid value) : base(value)
    {
    }
}