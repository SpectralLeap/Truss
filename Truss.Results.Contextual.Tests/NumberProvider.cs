namespace Truss.Results.Contextual.Tests;

internal sealed class NumberProvider
{
    private readonly int _value = Random.Shared.Next();
    private readonly int _delay = Random.Shared.Next(1, 100);
    
    public Number GetNumberSync(int i = 0)
    {
        return new Number(_value + i);
    }
        

    public async Task<Number> GetNumberAsync(int i = 0)
    {
        await Task.Delay(_delay).ConfigureAwait(false);
        
        return new Number(_value + i);
    }

    public async Task<Result<Number>> GetNumberResultAsync()
    {
        return await GetNumberAsync().ConfigureAwait(false);
    }
}