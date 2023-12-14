using Microsoft.Extensions.DependencyInjection;

namespace Truss.Core;

public sealed class IntegrationBus(IServiceProvider serviceProvider)
{
    private object? _lastResult;

    public void Apply<TAction, TResult>(params string[] args)
    {
        var handler = serviceProvider.GetService<IActionHandler<TAction, TResult>>();
        _lastResult = handler!.Handle(args);
    }
}