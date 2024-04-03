using Truss.Application.Abstractions.Domain;

namespace Truss.Application.Tests.EventSourcing.TestApplication;

public sealed record CounterId : AggregateRootId<Guid>
{
    public CounterId(Guid value) : base(value)
    {
    }
}