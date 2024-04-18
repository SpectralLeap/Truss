using Truss.Modeling.Application.DomainEvents;

namespace Truss.Modeling.Domain.Tests.Unit.Entities.TestDomain;

public sealed class WordUpdatedEventHandler : IDomainEventHandler<WordUpdatedEvent>
{
    private readonly WordListener _wordListener;

    public WordUpdatedEventHandler(WordListener wordListener)
    {
        _wordListener = wordListener;
    }
    
    public Task Handle(WordUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _wordListener.Word = notification.Word;
            
        return Task.CompletedTask;
    }
}

// ReSharper disable once UnusedType.Global
// this is used through MediatR