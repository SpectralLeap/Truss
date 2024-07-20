using Truss.Modeling.Application.DomainEvents;

namespace Truss.Modeling.Domain.Tests.Unit.Entities.TestDomain;

public class NumberUpdatedEventHandler : IDomainEventHandler<NumberUpdatedEvent>
{
    private readonly NumberListener _numberListener;

    public NumberUpdatedEventHandler(NumberListener numberListener)
    {
        _numberListener = numberListener;
    }
    
    public Task Handle(NumberUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _numberListener.Number = notification.Number;
        _numberListener.Numbers.Push(notification.Number);
            
        return Task.CompletedTask;
    }
}