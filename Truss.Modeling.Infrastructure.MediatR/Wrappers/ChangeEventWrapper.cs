using MediatR;

namespace Truss.Modeling.Infrastructure.MediatR.Wrappers;

internal sealed class ChangeEventWrapper<TChangeEvent>(TChangeEvent changeEvent) : INotification
{
    public TChangeEvent ChangeEvent { get; } = changeEvent;
}