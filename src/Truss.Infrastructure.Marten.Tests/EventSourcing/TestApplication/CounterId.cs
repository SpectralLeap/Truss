using Truss.Modeling.Domain.Entities;

namespace Truss.Infrastructure.Marten.Tests.EventSourcing.TestApplication;

public sealed record CounterId : AggregateId<Guid>
{
    public CounterId(Guid value) : base(value)
    {
    }
}