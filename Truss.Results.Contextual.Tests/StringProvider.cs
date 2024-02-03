namespace Truss.Results.Contextual.Tests;

internal sealed class StringProvider
{
    private readonly string _value = Guid.NewGuid().ToString();
    private readonly int _delay = Random.Shared.Next(10, 100);

    public String GetStringSync()
    {
        return new String(_value);
    }

    public Result<String> GetStringResult()
    {
        return Result.Success(GetStringSync());
    }
    
    public async Task<String> GetStringAsync(String? s = null)
    {
        await Task.Delay(_delay).ConfigureAwait(false);

        return new String(s?.Value ?? _value);
    }
    
    public async Task<Result<String>> GetStringResultAsync()
    {
        return await GetStringAsync().ConfigureAwait(false);
    }
}