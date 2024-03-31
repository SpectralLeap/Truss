using System;
using System.Threading.Tasks;

namespace Truss.Results.Extensions.Fluent.Tests;

public sealed class Tests
{
    public void DoesThing()
    {
        var x = 3.AsResult()
            .Then(i => i + 1)
            .ThenAsync(x => Task.FromResult(2 + 1))
            .DoAsync(x => Console.WriteLine())
            .AndAsync(x => x + 1)
            .AndAsync((a, b) => a + 2)
            .AndAsync(_ => 4 + 3)
            .ThenAsync((x, y, z, r) => Task.FromResult( x + y+ z + r))
            ;
    }
}