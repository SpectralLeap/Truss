using System;

namespace Truss.Modeling.Area;

public interface IAreaInstaller
{
    public void InstallModules(IModuleAggregator moduleAggregator);
}