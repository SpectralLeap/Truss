using System;
using System.Threading.Tasks;
using Xunit;

namespace Truss.Results.Extensions.Fluent.Tests;

public sealed class Tests
{
    
    [Fact]
    public void DoesThing()
    {
        var x = 3.AsResult()
            .Then(i => i + 1)
            .ThenTaskAsync(x => Task.FromResult(x + 1))
            .DoAsync(x => Console.WriteLine())
            .AndAsync(x => x + 1)
            .AndAsync((a, b) => a + 2)
            .AndAsync(_ => 4 + 3)
            .AndAsync(_ => Task.FromResult(4 + 3))
            .ThenTaskAsync((x, y, z, r) =>
            {
                return Task.FromResult(x + y + z + r);
            })
            ;
    }
}