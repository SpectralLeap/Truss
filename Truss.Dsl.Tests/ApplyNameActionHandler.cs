using Truss.Core;

namespace Truss.Dsl.Tests;

public sealed class ApplyNameActionHandler : IActionHandler<ApplyNameAction, ApplyNameResult>
{
    public ApplyNameResult Handle(params string[] args)
    {
        return new ApplyNameResult();
    }
}