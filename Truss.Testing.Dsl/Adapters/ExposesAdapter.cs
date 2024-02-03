namespace Truss.Testing.Dsl.Adapters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ExposesAdapter<T> : Attribute
{
    public string AdapterName { get; }

    public ExposesAdapter(string adapterName)
    {
        AdapterName = adapterName;
    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DependsOnAdapter<T> : Attribute where T : Dsl;