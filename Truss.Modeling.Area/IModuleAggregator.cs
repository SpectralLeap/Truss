using Truss.Modeling.Module;

namespace Truss.Modeling.Area;

public interface IModuleAggregator
{
    public IModuleAggregator AddModule<TModuleDefinition>() 
        where TModuleDefinition : IModuleInstaller;
}