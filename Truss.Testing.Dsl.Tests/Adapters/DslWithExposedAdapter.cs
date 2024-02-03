using Truss.Testing.Dsl.Adapters;
using Truss.Testing.Dsl.Tests.Services;

namespace Truss.Testing.Dsl.Tests.Adapters;

[ExposesAdapter<HttpClient>(AdapterName)] 
[DependsOnAdapter<OtherDslWithExposedAdapter>]
public sealed class DslWithExposedAdapter : Dsl
{
    public const string AdapterName = "Adapter 1";
}

[ExposesAdapter<HttpClient>(AdapterName)]
public sealed class OtherDslWithExposedAdapter : Dsl
{
    public const string AdapterName = "Adapter 2";
}




