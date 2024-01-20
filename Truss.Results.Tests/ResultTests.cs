using Truss.Results.Functional;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS0162 // Unreachable code detected

namespace Truss.Results.Tests;

public sealed class ResultTests
{
    [Fact]
    public void PassesSuccessWhenResultOfTAndResult()
    {
        var x = Result.Success("some")
            .Then(s => s.Length)
            .Then(i => i == 4 ? Result.Success(2) : Result.Fail("No bueno"))
            .Then(i => i + 8)
            .Then(_ => Result.Success())
            .Then(_ => Result.Success("x"));
        
        Assert.Equal("x", x.ExpectSuccessAndGet());
    }
    
    [Fact]
    public void PassesFailure()
    {
        var x = Result.Fail("some")
            .Then(s => Result.Success(s.Length))
            .Then(i => i == 4 ? Result.Success() : Result.Fail("No bueno"))
            .Then(_ => Result.Success())
            .Then(_ => Result.Success("x"));
            
        x.AssertFailure();
        Assert.Contains("some", x.ExpectFailureAndGet().FailureReasons);
    }
    
    [Fact]
    public void PassesFailureWithException()
    {
        var x = Result.Fail(new Exception("No bueno"));
                    
        x.AssertFailure();
        Assert.Contains("No bueno", x.ExpectFailureAndGet().FailureReasons);
    }
     
    [Fact]
    public void PassesFailureWithMessageAndException()
    {
        var x = Result.Fail("some", new Exception("No bueno"));
                
        x.AssertFailure();
        Assert.Contains("some", x.ExpectFailureAndGet().FailureReasons);
        Assert.Contains("No bueno", x.ExpectFailureAndGet().FailureReasons);
    }
     
    [Fact]
    public void CanResultTypes()
    {
        var x = Result.Success(1)
                .Then(i => Result.Success(i + 1))
                .Then(i => Result.Success(i + 1))
                .Then(i => Result.Success(i + 1))
                .Then(i => Result.Success(i + 1))
            ;
                
        Assert.Equal(5, x.ExpectSuccessAndGet());
    }
    
    [Fact]
    public void CanDoManyResultsPermutation()
    {
        var x = Result.Success(1)
            .Then(i => Result.Success(i + 1))
            .Then(i => Result.Success(i + 2));
                
        Assert.Equal(4, x.ExpectSuccessAndGet());
    }
 
    [Fact]
    public void FailsThroughManyResults()
    {
        var x = Result.Fail("bad")
            .Then(i => Result.Success(i + 2));
                    
        x.AssertFailure();
        Assert.Contains("bad", x.ExpectFailureAndGet().FailureReasons);
    }
    
    [Fact]
    public void BindResultToResult()
    {
        var x = Result.Success().Then(_ => Result.Success());
        x.AssertSuccessful();
    }
    
    [Fact]
    public void CanFailForManyReasons()
    {
    
        var x = Result.Success()
            .Then(_ => Result.Fail(new[] {"no", "not good"}));
        
        x.AssertFailure();
        
        Assert.Equal(new[] {"no", "not good"}, x.ExpectFailureAndGet().FailureReasons);
    }
    
    [Fact]
    public void CapturesExceptionsInFailureBinding()
    {
        
        var x = Result.Success().Then(_ =>
        {
            throw new Exception("bad bad not good");
            return Result.Success();
        });
        
        x.AssertFailure();
        Assert.Contains("bad bad not good", x.ExpectFailureAndGet().FailureReasons);
        Assert.NotNull(x.ExpectFailureAndGet().Exception);
    }
    
    [Fact]
    public void CanMapAResultOut()
    {
       
        var x = Result.Success("1")
            .Then(i => Result.Success(int.Parse(i)));
        
        x.AssertSuccessful();
        Assert.Equal(1, x.ExpectSuccessAndGet());
    }
    
    [Fact]
    public void NullForSuccessThrowsAnInvalidState()
    {
        Assert.Throws<InvalidResultStateException>((Action)(() => 
            Result.Success(null)
                .Then(_ => Result.Success(1))));
    }
     
    [Fact]
    public void CastsBetweenUnitAndTypesOfT()
    {
        // This shouldn't compile if broken so just assert success
        var x = Result.Success("1")
                .Then(_ => Result.Success())
                .Then(_ => Result.Success())
                .Then(_ => Result.Success("value"))
                .Then(s => Result.Success(s))
            ;
                
        x.AssertSuccessful();
    }
    
    [Fact]
    public void MatchingOnNullThrows()
    {
        Assert.Throws<InvalidResultStateException>((Action)(() =>
                Result.Success(null)
                    .Resolve(
                        forSuccess: _ => Result.Success("["),
                        forFailure: _ => Result.Fail("bad bad not good"))))
            ;
    }
        
    [Fact]
    public async Task CanDoAsyncMatch()
    {
        var x = await Result.Success("1")
                .Resolve(
                    forSuccess: async s => await Task.Run(() => Task.FromResult(Result.Success(s))),
                    forFailure: async _ => await Task.Run(() => Task.FromResult(Result.Fail("bad bad not good"))))
            ;

        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task CanDoAsyncMap()
    {
        var x = await Result.Success("1")
                .Then(s => Task.FromResult(Result.Success(s)))
                .Then(async s => await Task.Run(() => Task.FromResult(Result.Success(s))))
            ;
    
        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task CanDoAsyncMapFromResultTResult()
    {
        var x = await Result.Success("1")
                .Then(s => Task.FromResult(Result.Success(s)))
                .Then(async s => await Task.Run(() => Task.FromResult(Result.Success(s))))
                .Then(_ => Task.FromResult(Result.Success("yay")))
            ;
        
        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task FailureWorksWithAsyncStuff()
    {
        var x = await Result.Fail("nope")
                .Then(_ => Task.FromResult(Result.Success(2)))
                .Then(async i => await Task.Run(() => Task.FromResult(Result.Success($"{i}"))))
                .Then(_ => Task.FromResult(Result.Success("yay")))
            ;
            
        Assert.Contains("nope", x.ExpectFailureAndGet().FailureReasons);
    }
    
    [Fact]
    public async Task CanDoMatchTask()
    {
        await Result.Success()
                .Resolve(
                    forSuccess: async _ => await Task.Run(() => Assert.True(true)),
                    forFailure: async _ => await Task.Run(() => Assert.Fail("bad bad not good")))
            ;
    }
    
    [Fact]
    public async Task MatchTaskThatThrowsReportsException()
    {
        await Result.Success()
                .Resolve(
                    forSuccess: async _ =>
                    {
                        throw new Exception("hi");
                        await Task.Run(() => Assert.True(true));
                    },
                    forFailure: async f => await Task.Run(() => Assert.Equal("hi", f.Exception!.Message)))
            ;
    }
    
    [Fact]
    public async Task CannotResolveNullSuccess()
    {
        await Assert.ThrowsAsync<InvalidResultStateException>(async () => 
            await Result.Success((string)null)
                .Resolve(
                    forSuccess: _ => Task.FromResult(Task.FromResult(1)),
                    forFailure: _ => Task.FromResult(Task.FromResult(2))));
    }
        
    [Fact]
    public async Task MatchTaskFromFailedDoesNotEvaluateSuccess()
    {
        await Result.Fail()
                .Resolve(
                    forSuccess: async _ =>
                    {
                        await Task.Run(() => Assert.True(false, "Should not succeed"));
                    },
                    forFailure: async _ => await Task.Run(() => Assert.True(true)))
            ;
    }
      
        
    [Fact]
    public async Task MakingSuccessValueNullCreatesInvalidState()
    {
        await Assert.ThrowsAsync<InvalidResultStateException>(async () =>
        {
            await Result.Success((string)null)
                    .Resolve(
                        forSuccess: async _ => { await Task.Run(() => Assert.True(false, "Should not succeed")); },
                        forFailure: async _ => await Task.Run(() => Assert.True(true)))
                ;
        });
    }
 
    [Fact]
    public async Task CanDoAsyncMapFromResultToResult()
    {
        var x = await Result.Success("1")
                .Then(_ => Task.FromResult(Result.Success()))
            ;

        var y = await x.Then(_ => Task.FromResult(Result.Success()));
        
            
        x.AssertSuccessful();
        y.AssertSuccessful();
    }
     
    [Fact]
    public async Task CanMapTaskResultToResult()
    {
        // This should compile in this structure
        Result x = await (await DoThing())
                .Then(async _ => await DoOtherThing())
                .Then(_ => Result.Success())
            ;

        x.AssertSuccessful();
    }

    [Fact]
    public async Task CanMapTaskResultToResultT()
    {
        // This should compile in this structure
        Result<string> x = await (await DoThing())
                .Then(async _ => await DoOtherThing())
                .Then(_ => Result.Success("k"))
            ;
    
        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task CanMapTaskResultToTaskResult()
    {
        // This should compile in this structure
        Result x = await (await DoThing())
                .Then(async _ => await DoOtherThing())
                .Then(async _ => await DoOtherThing())
            ;
    
        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task CanDoTaskThenTaskOfT()
    {
        // This should compile in this structure
        Result x = await (await DoThing())
                .Then(async _ => await DoOtherThing())
                .Then(async _ => await DoOtherThing())
            ;
    
        x.AssertSuccessful();
    }
    
    [Fact]
    public async Task CanDoTaskOfTAndThenATaskAndUseMethodBodies()
    {
        // This should compile in this structure
        var x = await (await DoThing())
                .Then(async _ => await DoThing())
                .Then(async _ => await DoThing())
                .Then(async _ => await DoOtherThing())
                .Then(async _ => await DoThing())
                .Then(DoStringThing)
                .Then(async _ => await DoOtherThing())
                .Then(DoThing)
            ;
    
        x.AssertSuccessful();
    }

    [Fact]
    public void CanDoImplicitFlatMapping()
    {
        // This should compile in this structure
        var x = Result.Success()
                .Then(_ => "x")
                .Then(_ => 4)
            ;
            
        Assert.Equal(4, x.ExpectSuccessAndGet());
    }

    [Fact]
    public void CanDoImplicitFlatMappingWithActions()
    {
        // This should compile in this structure
        var x = Result.Success()
                .Then(_ => "x")
                .Then(_ => new List<string>().Add("as"))
                .Then(_ => 4)
            ;
                
        Assert.Equal(4, x.ExpectSuccessAndGet());
    }

    [Fact]
    public void ClosuresPersistValues()
    {
        var list = new List<string>();
        // This should compile in this structure
        Result.Success()
            .Then(_ => "x")
            .Then(s => list.Add(s))
            .Then(_ => 4);
                
        Assert.Contains("x", list);
    }
    
    [Fact]
    public async Task CanDoImplicitFlatMappingAsync()
    {
        // This should compile in this structure
        var x = await DoThing()
                .Then(async _ => await DoThing())
                .Then(_ => "4")
                .Then(int.Parse)
            ;
        
        Assert.Equal(4, x.ExpectSuccessAndGet());
    }

    [Fact]
    public void ImplicitlyCastsToTask()
    {
        Task<Result> _ = Result.Success();
            
        Assert.True(true);
    }
    
    [Fact]
    public void ImplicitlyCastsResultOfTToTask()
    {
        Task<Result.Success("k");
            
        Assert.True(true);
    }

    [Fact]
    public async Task ExceptionsInAsyncReturnFailure()
    {
        // This should compile in this structure
        var x = await DoThing()
                .Then(async _ =>
                {
                    throw new Exception("hi");
                    return await DoThing();
                })
                .Then(_ => "4")
                .Then(int.Parse)
            ;

        Assert.Equal("hi", x.ExpectFailureAndGet().Exception!.Message);
    }

    [Fact]
    public async Task CanDoImplicitFlatMappingAsyncInClojureWithActions()
    {
        var list = new List<string>();
        // This should compile in this structure
        await DoThing()
            .Then(async _ => await DoThing())
            .Then(s => $"Good {s}")
            .Then(s => list.Add(s));
            
        Assert.Contains("Good k", list);
    }

    [Fact]
    public async Task DoesNotCreateResultOfResultOfT()
    {
        var list = new List<string>();
        // This should compile in this structure
        var x = await DoThing()
                .Then(s => Result.Success($"Good {s}"))
                .Then(s => list.Add(s))
            ;
                
        x.AssertSuccessful();
        Assert.Contains("Good k", list);
    }

    [Fact]
    public async Task DirectlyResolveTasksOfResultOfT()
    {
        // This should compile in this structure
        var x = await DoThing()
                .Resolve(
                    forSuccess: _ => 1,
                    forFailure: _ => 2)
            ;
                    
        Assert.Equal(1, x);
    }

    [Fact]
    public async Task DirectlyResolveTasksOfResult()
    {
        // This should compile in this structure
        var x = await DoOtherThing()
                .Resolve(
                    forSuccess: _ => 1,
                    forFailure: _ => 2)
            ;
                        
        Assert.Equal(1, x);
    }
    
    [Fact]
    public async Task DirectlyResolveTasksOfResultWithTask()
    {
        // This should compile in this structure
        var x = await DoOtherThing()
                .Resolve(
                    forSuccess: _ => Task.FromResult(1),
                    forFailure: _ => Task.FromResult(2))
            ;
                         
        Assert.Equal(1, await x);
    }
     
    [Fact]
    public async Task DirectlyResolveTasksOfResultOfTWithTask()
    {
        // This should compile in this structure
        var x = await DoThing()
                .Resolve(
                    forSuccess: _ => Task.FromResult(1),
                    forFailure: _ => Task.FromResult(2))
            ;
                          
        Assert.Equal(1, await x);
    }
    
    [Fact]
    public async Task ResolveTaskWithNonTask()
    {
        // This should compile in this structure
        var x = await DoThing()
                .Then(v => Task.FromResult(v))
                .Resolve<string, int>(
                    forSuccess: v => 1,
                    forFailure: _ => 2)
            ;
                           
        Assert.Equal(1, x);
    }
       
    [Fact]
    public void CanUseIfThenForSuccessfulResult()
    {
        var x = Result.Success("k");
        
        Assert.True(x.Succeeded);
        Assert.Equal("k", x.SuccessValue);
    }
     
    [Fact]
    public void CanUseIfThenForFailedResult()
    {
        var x = Result.Fail("Not good");
            
        Assert.True(x.Failed);
        Assert.Equal(new[]{"Not good"}, x.FailureDetails.FailureReasons);
    }
     
    [Fact]
    public void ThrowsWhenGettingSuccessValueForFailed()
    {
        var x = Result.Fail("Not good");
                
        Assert.Throws<InvalidOperationException>(() => x.SuccessValue);
    }
    
    [Fact]
    public void ThrowsWhenGettingFailureValueForSucceeded()
    {
        var x = Result.Success("Not good");
                    
        Assert.Throws<InvalidOperationException>(() => x.FailureDetails);
    }
     
    [Fact]
    public async Task CanChainWithAndOnce()
    {
        var expectedString = "alright";
        
        var x1 = 
                Result.Success(expectedString)
                    .And(_ => Result.Success(1))
                    .Then((s, i) => Result.Success($"{s} {i}"))
            ;
        
        var x2 = 
                Result.Success(expectedString)
                    .And(_ => 1)
                    .Then((s, i) => $"{s} {i}")
            ;
                         
        var x3 = 
                await Task.FromResult(Result.Success(expectedString))
                    .And(_ => Task.FromResult(Result.Success(1)))
                    .Then((s, i) => $"{s} {i}")
            ;
         
        var x4 = 
                await Result.Success(expectedString)
                    .And(_ => Task.FromResult(Result.Success(1)))
                    .Then((s, i) => $"{s} {i}")
            ;
         
        var x5 = 
                await Result.Success(expectedString)
                    .And(_ => Task.FromResult(1))
                    .Then((s, i) => $"{s} {i}")
            ;
        
        var x6 = 
                await Task.FromResult(Result.Success(expectedString))
                    .And(_ => Task.FromResult(1))
                    .Then((s, i) => $"{s} {i}")
            ;
        
        var x7 = 
                await Task.FromResult(Result.Success(expectedString))
                    .And(_ => (1))
                    .Then((s, i) => $"{s} {i}")
            ;        
        
        Assert.Equal($"{expectedString} 1", x1.SuccessValue);
        Assert.Equal($"{expectedString} 1", x2.SuccessValue);
        Assert.Equal($"{expectedString} 1", x3.SuccessValue);
        Assert.Equal($"{expectedString} 1", x4.SuccessValue);
        Assert.Equal($"{expectedString} 1", x5.SuccessValue);
        Assert.Equal($"{expectedString} 1", x6.SuccessValue);
        Assert.Equal($"{expectedString} 1", x7.SuccessValue);
    }
     
    [Fact]
    public async Task CanChainWithAndTwice()
    {
        var expectedString = "a";
        
        var x1 = 
                Result.Success(expectedString)
                    .And(_ => Result.Success(1))
                    .And((_, _) => Result.Success(1))
                    .Then((s, i, c) => Result.Success($"{s} {i} {c}"))
            ;
        
        var x2 = 
                Result.Success(expectedString)
                    .And(_ => 1)
                    .And((_, _) => 1)
                    .Then((s, i, c) => $"{s} {i} {c}")
            ;
                         
        var x3 = 
                await Task.FromResult(Result.Success(expectedString))
                    .And(_ => Task.FromResult(Result.Success(1)))
                    .And((_, _) => Task.FromResult(Result.Success(1)))
                    .Then((s, i, c) => $"{s} {i} {c}")
            ;
         
        var x4 = 
                await Result.Success(expectedString)
                    .And(_ => Result.Success(1))
                    .And((_, _) => Task.FromResult(1))
                    .Then((s, i, c) => $"{s} {i} {c}")
            ;
         
        var x5 = 
                await Result.Success(expectedString)
                    .And(_ => Result.Success(1))
                    .And((_, _) => Task.FromResult(Result.Success(1)))
                    .Then((s, i, c) => $"{s} {i} {c}")
            ;
        
        var x6 = 
                await Result.Success(expectedString)
                    .And(_ => Task.FromResult(Result.Success(1)))
                    .And((_, _) => Task.FromResult(1))
                    .Then((s, i, c) => $"{s} {i} {c}")
            ;
         
        Assert.Equal($"{expectedString} 1 1", x1.SuccessValue);
        Assert.Equal($"{expectedString} 1 1", x2.SuccessValue);
        Assert.Equal($"{expectedString} 1 1", x3.SuccessValue);
        Assert.Equal($"{expectedString} 1 1", x4.SuccessValue);
        Assert.Equal($"{expectedString} 1 1", x5.SuccessValue);
        Assert.Equal($"{expectedString} 1 1", x6.SuccessValue);
    }
     
    [Fact]
    public async Task CanChainWithAndThrice()
    {
        var expectedString = "a";
            
        var x1 = 
                Result.Success(expectedString)
                    .And(_ => Result.Success(1))
                    .And((_, _) => Result.Success(1))
                    .And((_, _, _) => Result.Success(1))
                    .Then((s, i, c, d) => Result.Success($"{s} {i} {c} {d}"))
            ;
            
        var x2 = 
                Result.Success(expectedString)
                    .And(_ => 1)
                    .And((_, _) => 1)
                    .And((_, _, _) => 1)
                    .Then((s, i, c, d) => $"{s} {i} {c} {d}")
            ;
                             
        var x3 = 
                await Task.FromResult(Result.Success(expectedString))
                    .And(_ => Task.FromResult(Result.Success(1)))
                    .And((_, _) => Task.FromResult(Result.Success(1)))
                    .And((_, _, _) => Task.FromResult(Result.Success(1)))
                    .Then((s, i, c, d) => $"{s} {i} {c} {d}")
            ;
             
        var x4 = 
                await Result.Success(expectedString)
                    .And(_ => Task.FromResult(Result.Success(1)))
                    .And((_, _) => Task.FromResult(Result.Success(1)))
                    .And((_, _, _) => Task.FromResult(Result.Success(1)))
                    .Then((s, i, c, d) => $"{s} {i} {c} {d}")
            ;
             
        var x5 = 
                await Result.Success(expectedString)
                    .And(_ => Task.FromResult(1))
                    .And((_, _) => Task.FromResult(1))
                    .And((_, _, _) => Task.FromResult(1))
                    .Then((s, i, c, d) => $"{s} {i} {c} {d}")
            ;
        
        var x6 = 
                await Result.Success(expectedString)
                    .And(_ => Task.FromResult(Result.Success(1)))
                    .And((_, _) => Task.FromResult(1))
                    .And((_, _, _) => Task.FromResult(1))
                    .Then((s, i, c, d) => $"{s} {i} {c} {d}")
            ;
             
        Assert.Equal($"{expectedString} 1 1 1", x1.SuccessValue);
        Assert.Equal($"{expectedString} 1 1 1", x2.SuccessValue);
        Assert.Equal($"{expectedString} 1 1 1", x3.SuccessValue);
        Assert.Equal($"{expectedString} 1 1 1", x4.SuccessValue);
        Assert.Equal($"{expectedString} 1 1 1", x5.SuccessValue);
        Assert.Equal($"{expectedString} 1 1 1", x6.SuccessValue);
    }
    
    [Fact]
    public async Task CanChainFourWholeTimes()
    {
        var expectedString = "a";
                 
        var x1 = 
                Result.Success(expectedString)
                    .And(_ => Result.Success(1))
                    .And((_, _) => Result.Success(1))
                    .And((_, _, _) => Result.Success(1))
                    .And((_, _, _, _) => Result.Success(1))
                    .Then((s, i, c, d, e) => Result.Success($"{s} {i} {c} {d} {e}"))
            ;
                 
        var x2 = 
                Result.Success(expectedString)
                    .And(_ => 1)
                    .And((_, _) => 1)
                    .And((_, _, _) => 1)
                    .And((_, _, _, _) => 1)
                    .Then((s, i, c, d, e) => $"{s} {i} {c} {d} {e}")
            ;
                                  
        var x3 = 
                await Task.FromResult(Result.Success(expectedString))
                    .And(_ => Task.FromResult(Result.Success(1)))
                    .And((_, _) => Task.FromResult(Result.Success(1)))
                    .And((_, _, _) => Task.FromResult(Result.Success(1)))
                    .And((_, _, _, _) => Task.FromResult(Result.Success(1)))
                    .Then((s, i, c, d, e) => $"{s} {i} {c} {d} {e}")
            ;
                  
        var x4 = 
                await Result.Success(expectedString)
                    .And(_ => Task.FromResult(Result.Success(1)))
                    .And((_, _) => Task.FromResult(Result.Success(1)))
                    .And((_, _, _) => Task.FromResult(Result.Success(1)))
                    .And((_, _, _, _) => Task.FromResult(Result.Success(1)))
                    .Then((s, i, c, d, e) => $"{s} {i} {c} {d} {e}")
            ;
                  
        var x5 = 
                await Result.Success(expectedString)
                    .And(_ => Task.FromResult(1))
                    .And((_, _) => Task.FromResult(1))
                    .And((_, _, _) => Task.FromResult(1))
                    .And((_, _, _, _) => Task.FromResult(1))
                    .Then((s, i, c, d, e) => $"{s} {i} {c} {d} {e}")
            ;
             
        var x6 = 
                await Result.Success(expectedString)
                    .And(_ => Task.FromResult(Result.Success(1)))
                    .And((_, _) => Task.FromResult(1))
                    .And((_, _, _) => Task.FromResult(1))
                    .And((_, _, _, _) => Task.FromResult(1))
                    .Then((s, i, c, d, e) => $"{s} {i} {c} {d} {e}")
            ;
                  
        Assert.Equal($"{expectedString} 1 1 1 1", x1.SuccessValue);
        Assert.Equal($"{expectedString} 1 1 1 1", x2.SuccessValue);
        Assert.Equal($"{expectedString} 1 1 1 1", x3.SuccessValue);
        Assert.Equal($"{expectedString} 1 1 1 1", x4.SuccessValue);
        Assert.Equal($"{expectedString} 1 1 1 1", x5.SuccessValue);
        Assert.Equal($"{expectedString} 1 1 1 1", x6.SuccessValue);
    }
    
    [Fact]
    public void ResolveOrThrowReturnsValue()
    {
        var x = Result.Success("some")
            .ResolveOrThrow()
            ;
         
        Assert.Equal("some", x);
    }
       
    [Fact]
    public void ResolveOrThrowThrowsFailureException()
    {
        Assert.Throws<OverflowException>(() => 
                Result.Success("some")
                .Then(s =>
                {
                    throw new OverflowException("No good");
                    return s;
                })
                .ResolveOrThrow());
    }
     
    [Fact]
    public void IfNoExceptionResolveOrThrowsThrowsDefinedException()
    {
        Assert.Throws<ResolutionException>(() => 
            Result.Success("some")
                .Then(s => Result.Fail("invalid"))
                .ResolveOrThrow());
    }
    
    [Fact]
    public void IfDefinedExceptionThrowsItHasErrorDetails()
    {
        var e = Assert.Throws<ResolutionException>(() => 
            Result.Success("some")
                .Then(s => Result.Fail("invalid", "no good"))
                .ResolveOrThrow());

        Assert.Contains("invalid", e.Message);
        Assert.Contains("no good", e.Message);
    }
    
    [Fact]
    public async Task ResolveOrThrowReturnsValueAsync()
    {

    }
        
       
    private Task<Result<string>> DoThing()
    {
        return Task.FromResult(Result.Success("k"));
    }
    
    private Task<Result<string>> DoStringThing(string s)
    {
        return Task.FromResult(Result.Success(s));
    }
     
    private Task<Result> DoOtherThing()
    {
        return Task.FromResult(Result.Success());
    }
}