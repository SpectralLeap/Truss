using Truss.Application.Abstractions.Domain;

namespace Truss.Application.Tests.Unit.EventSourcing.TestApplication;

public sealed record CounterId : AggregateRootId<Guid>
{
    public CounterId(Guid value) : base(value)
    {
    }
}