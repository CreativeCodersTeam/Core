using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure;

[PublicAPI]
public interface IRegionManager
{
    IRegion AddRegion(string regionName);

    void RemoveRegion(string regionName);

    void RemoveRegion(IRegion region);

    IEnumerable<IRegion> Regions { get; }

    void AddToRegion(string regionName, object view);

    void RegisterRegionView(string regionName, Type viewType);
}
