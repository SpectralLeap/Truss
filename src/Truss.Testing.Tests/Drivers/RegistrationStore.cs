namespace Truss.Testing.Tests.Drivers;

public sealed class RegistrationStore
{
    private readonly List<string> _dataBase = new();
    
    public void AddData(string data)
    {
        _dataBase.Add(data);    
    }

    public bool Has(string data)
    {
        return _dataBase.Contains(data);
    }
}