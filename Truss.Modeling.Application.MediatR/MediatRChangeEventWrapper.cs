using MediatR;

namespace Truss.Modeling.Application.MediatR;

internal sealed class MediatRChangeEventWrapper<TChangeEvent>(TChangeEvent changeEvent) : INotification
{
    public TChangeEvent ChangeEvent { get; } = changeEvent;
}